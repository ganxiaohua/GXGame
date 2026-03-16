//========================================================
//--------------|         INTERRA         |---------------
//========================================================
//--------------|          3.8.0          |---------------
//========================================================
//--------------| ©  INEFFABILIS ARCANUM  |---------------
//========================================================

using System.Collections.Generic;
using UnityEngine;
using System.Linq;

#if UNITY_EDITOR
	using UnityEditor;
	using UnityEngine.Rendering;
#endif

namespace InTerra
{
	public static class InTerra_Data
	{
		public const string ObjectShaderName = "InTerra/Object into Terrain Integration";
		public const string DiffuseObjectShaderName = "InTerra/Diffuse/Object into Terrain Integration (Diffuse)";
		public const string URPObjectShaderName = "InTerra/URP/Object into Terrain Integration";
		public const string HDRPObjectShaderName = "InTerra/HDRP/Object into Terrain Integration";
		public const string HDRPObjectTessellationShaderName = "InTerra/HDRP Tessellation/Object into Terrain Integration Tessellation";

		public const string TerrainShaderName = "InTerra/Terrain (Standard With Features)";
		public const string DiffuseTerrainShaderName = "InTerra/Diffuse/Terrain (Diffuse With Features)";
		public const string URPTerrainShaderName = "InTerra/URP/Terrain (Lit with Features)";
		public const string HDRPTerrainShaderName = "InTerra/HDRP/Terrain (Lit with Features)";
		public const string HDRPTerrainTessellationShaderName = "InTerra/HDRP Tessellation/Terrain (Lit with Features)";

		public const string TessellationShaderFolder = "InTerra/HDRP Tessellation";

		const string UpdaterName = "InTerra_UpdateAndCheck";
		static GameObject updater;
		static InTerra_UpdateAndCheck updateScript;

		static bool shaderVariantWarning;

		public static void UpdateTerrainData(bool UpdateDictionary)
		{
			Terrain[] terrains = Terrain.activeTerrains;
			if (terrains.Length > 0)
			{
				DictionaryMaterialTerrain materialTerrain = GetUpdaterScript().MaterialTerrain;

				if (UpdateDictionary)
				{
					//======= DICTIONARY OF MATERIALS WITH INTERRA SHADERS AND SUM POSITIONS OF RENDERERS WITH THAT MATERIAL =========
					Dictionary<Material, Vector3> matPos = new Dictionary<Material, Vector3>();
					#if UNITY_2023_1_OR_NEWER
						MeshRenderer[] renderers = Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
					#else
						MeshRenderer[] renderers = Object.FindObjectsOfType<MeshRenderer>();
					#endif

					foreach (MeshRenderer rend in renderers)
					{					
						if (rend != null && rend.bounds != null)
						{
							foreach (Material mat in rend.sharedMaterials)
							{
								if (CheckObjectShader(mat))
								{
									if (!matPos.ContainsKey(mat))
									{
										matPos.Add(mat, new Vector3(rend.bounds.center.x, rend.bounds.center.z, 1));
									}
									else
									{
										Vector3 sumPos = matPos[mat];
										sumPos.x += rend.bounds.center.x;
										sumPos.y += rend.bounds.center.z;
										sumPos.z += 1;
										matPos[mat] = sumPos;
									}
								}
							}
						}
					}
			
					//===================== DICTIONARY OF MATERIALS AND TERRAINS WHERE ARE PLACED =========================
					materialTerrain.Clear();

					foreach (Material mat in matPos.Keys)
					{
						Vector2 averagePos = matPos[mat] / matPos[mat].z;

						foreach (Terrain terrain in terrains)
						{
							if (CheckPosition(terrain, averagePos))
							{
								materialTerrain.Add(mat, terrain);
							}						
							if (terrain && terrain.materialTemplate.shader.name.Contains("InTerra/HDRP"))
							{
								terrain.materialTemplate.renderQueue = 2225;
							}
							if (terrain && terrain.materialTemplate.shader.name.Contains("InTerra/URP"))
							{
								#if UNITY_2022_2_OR_NEWER
									terrain.materialTemplate.EnableKeyword("UNITY_2022_2_OR_NEWER");
								#else
									terrain.materialTemplate.DisableKeyword("UNITY_2022_2_OR_NEWER");
								#endif
							}
						}
						if (!materialTerrain.ContainsKey(mat))
						{
							materialTerrain.Add(mat, null);
						}
					}
				}

				//================================================================================
				//--------------------|    SET TERRAINS DATA TO MATERIALS    |--------------------
				//================================================================================
				foreach (Material mat in materialTerrain.Keys)
				{
					Terrain terrain = materialTerrain[mat];
					if (terrain != null && CheckObjectShader(mat))
					{						
						mat.SetVector("_TerrainSize", terrain.terrainData.size); 
						mat.SetVector("_TerrainPosition", terrain.transform.position); 
						mat.SetVector("_TerrainHeightmapScale", new Vector4 (terrain.terrainData.heightmapScale.x, terrain.terrainData.heightmapScale.y / (32766.0f / 65535.0f), terrain.terrainData.heightmapScale.z, terrain.terrainData.heightmapScale.y));
						mat.SetTexture("_TerrainHeightmapTexture", terrain.terrainData.heightmapTexture);

						//-------------------|  InTerra Keywords  |------------------
						string[] keywords = new string[] 
						{   "_TERRAIN_MASK_MAPS",
							"_TERRAIN_BLEND_HEIGHT",
							"_TERRAIN_DISTANCEBLEND", 
							"_TERRAIN_NORMAL_IN_MASK",
							"_TERRAIN_PARALLAX",
							"_TERRAIN_TINT_TEXTURE"
						};

						if (terrain.materialTemplate.shader.name.Contains("InTerra/HDRP"))
						{
							if (terrain.terrainData.alphamapTextureCount > 1 && !(mat.IsKeywordEnabled("_LAYERS_ONE") && mat.IsKeywordEnabled("_LAYERS_TWO"))) mat.EnableKeyword("_LAYERS_EIGHT"); else mat.DisableKeyword("_LAYERS_EIGHT");
						}
				
						if (CheckTerrainShader(terrain.materialTemplate.shader))
						{					
							TerrainKeywordsToMaterial(terrain, mat, keywords);

							//------------------|  InTerra Properties  |------------------
							string[] floatProperties = new string[] 
							{	"_HT_distance_scale", 
								"_HT_cover",
								"_HeightTransition",
								"_Distance_HeightTransition",
								"_TriplanarOneToAllSteep",
								"_TriplanarSharpness",
								"_TerrainColorTintStrenght",
								"_TerrainNormalTintStrenght"
							};
							SetTerrainFloatsToMaterial(terrain, mat, floatProperties);
							SetTerrainVectorToMaterial(terrain, mat, "_HT_distance");
							SetTerrainVectorToMaterial(terrain, mat, "_TerrainNormalTintDistance");							
							SetTerrainTextureToMaterial(terrain, mat, "_TerrainColorTintTexture");
							SetTerrainTextureToMaterial(terrain, mat, "_TerrainNormalTintTexture");

							if ((mat.IsKeywordEnabled("_TERRAIN_PARALLAX") || terrain.materialTemplate.shader.name.Contains(TessellationShaderFolder)) && terrain.materialTemplate.shader.name != DiffuseTerrainShaderName)
							{
								mat.SetFloat("_MipMapLevel", terrain.materialTemplate.GetFloat("_MipMapLevel"));
								SetTerrainVectorToMaterial(terrain, mat, "_MipMapFade");
							}

							if (mat.shader.name == HDRPObjectTessellationShaderName && terrain.materialTemplate.shader.name.Contains(TessellationShaderFolder))
							{
								float terrainMaxDisplacement = terrain.materialTemplate.GetFloat("_TessellationMaxDisplacement");
								float objectMaxDisplacement = mat.GetFloat("_TessellationObjMaxDisplacement");

								mat.SetFloat("_TessellationMaxDisplacement", terrainMaxDisplacement > objectMaxDisplacement ? terrainMaxDisplacement : objectMaxDisplacement);

								string[] tessProperties = new string[]
								{   "_TessellationFactorMinDistance",
									"_TessellationFactorMaxDistance",
									"_Tessellation_HeightTransition",
									"_TessellationShadowQuality"
								};
								SetTerrainFloatsToMaterial(terrain, mat, tessProperties);
							}
						}
						else
						{
							string pipeline = terrain.materialTemplate.GetTag("RenderPipeline", false);
							if (pipeline == "UniversalPipeline" || pipeline == "HDRenderPipeline")
							{
								DisableKeywords(mat, keywords);
								mat.EnableKeyword("_TERRAIN_MASK_MAPS");
								if (terrain.materialTemplate.IsKeywordEnabled("_TERRAIN_BLEND_HEIGHT")) mat.EnableKeyword("_TERRAIN_BLEND_HEIGHT"); else mat.DisableKeyword("_TERRAIN_BLEND_HEIGHT");
								mat.SetFloat("_HeightTransition", 60 - 60 * terrain.materialTemplate.GetFloat("_HeightTransition"));
							}
							else
							{
								DisableKeywords(mat, keywords);
							}					
						}

						bool hasNormalMap = false;

						//----------- ONE PASS ------------
						if (!mat.IsKeywordEnabled("_LAYERS_TWO") && !mat.IsKeywordEnabled("_LAYERS_ONE") && !mat.IsKeywordEnabled("_LAYERS_EIGHT"))
						{
							int passNumber = (int)mat.GetFloat("_PassNumber");

							for (int i = 0; (i + (passNumber * 4)) < terrain.terrainData.alphamapLayers && i < 4; i++)
							{
								TerrainLaeyrDataToMaterial(terrain.terrainData.terrainLayers[i + ( passNumber * 4 )], i, mat);
								hasNormalMap = terrain.terrainData.terrainLayers[i + (passNumber * 4)].normalMapTexture || hasNormalMap;
							}

							if (terrain.terrainData.alphamapTextureCount > passNumber) mat.SetTexture("_Control", terrain.terrainData.alphamapTextures[passNumber]);
							if (passNumber > 0) mat.DisableKeyword("_TERRAIN_BLEND_HEIGHT");
						}

						//----------- ONE PASS (EIGHT LAYERS) ------------
						if (mat.IsKeywordEnabled("_LAYERS_EIGHT"))
						{
							int passNumber = (int)mat.GetFloat("_PassNumber");

							for (int i = 0; (i + (passNumber * 4)) < terrain.terrainData.alphamapLayers && i < 8; i++)
							{
								TerrainLaeyrDataToMaterial(terrain.terrainData.terrainLayers[i + (passNumber * 4)], i, mat);
								hasNormalMap = terrain.terrainData.terrainLayers[i + (passNumber * 4)].normalMapTexture || hasNormalMap;
							}

							if (terrain.terrainData.alphamapTextureCount > passNumber) mat.SetTexture("_Control", terrain.terrainData.alphamapTextures[0]);
							if (terrain.terrainData.alphamapTextureCount > passNumber) mat.SetTexture("_Control1", terrain.terrainData.alphamapTextures[1]);
							if (passNumber > 0) mat.DisableKeyword("_TERRAIN_BLEND_HEIGHT");							
						}

						//----------- ONE LAYER ------------
						if (mat.IsKeywordEnabled("_LAYERS_ONE"))
						{
							#if UNITY_EDITOR //The TerrainLayers in Editor are referenced by GUID, in Build by TerrainLayers array index
								TerrainLayer terainLayer = TerrainLayerFromGUID(mat, "TerrainLayerGUID_1");
								TerrainLaeyrDataToMaterial(terainLayer, 0, mat);
								hasNormalMap = terainLayer && terainLayer.normalMapTexture;
							#else
								int layerIndex1 = (int)mat.GetFloat("_LayerIndex1");
								CheckLayerIndex(terrain, 0, mat, ref layerIndex1);
								TerrainLaeyrDataToMaterial(terrain.terrainData.terrainLayers[layerIndex1], 0, mat);	
								hasNormalMap = terrain.terrainData.terrainLayers[layerIndex1].normalMapTexture;
							#endif
						}
				
						//----------- TWO LAYERS ------------
						if (mat.IsKeywordEnabled("_LAYERS_TWO"))
						{
							#if UNITY_EDITOR
								TerrainLayer terainLayer1 = TerrainLayerFromGUID(mat, "TerrainLayerGUID_1");
								TerrainLayer terainLayer2 = TerrainLayerFromGUID(mat, "TerrainLayerGUID_2");
								TerrainLaeyrDataToMaterial(terainLayer1, 0, mat);
								TerrainLaeyrDataToMaterial(terainLayer2, 1, mat);
								int layerIndex1 = terrain.terrainData.terrainLayers.ToList().IndexOf(terainLayer1);
								int layerIndex2 = terrain.terrainData.terrainLayers.ToList().IndexOf(terainLayer2);
								hasNormalMap = terainLayer1 && terainLayer2 && (terainLayer1.normalMapTexture || terainLayer2.normalMapTexture);
							#else
								int layerIndex1 = (int)mat.GetFloat("_LayerIndex1"); 
								int layerIndex2 = (int)mat.GetFloat("_LayerIndex2");
								CheckLayerIndex(terrain, 0, mat, ref layerIndex1);
								CheckLayerIndex(terrain, 1, mat, ref layerIndex2);
								TerrainLaeyrDataToMaterial(terrain.terrainData.terrainLayers[layerIndex1], 0, mat);
								TerrainLaeyrDataToMaterial(terrain.terrainData.terrainLayers[layerIndex2], 1, mat);	
								hasNormalMap = terrain.terrainData.terrainLayers[layerIndex1].normalMapTexture || terrain.terrainData.terrainLayers[layerIndex2].normalMapTexture;
							#endif

							mat.SetFloat("_ControlNumber", layerIndex1 % 4); 

							if (terrain.terrainData.alphamapTextureCount > layerIndex1 / 4) mat.SetTexture("_Control", terrain.terrainData.alphamapTextures[layerIndex1 / 4]);
							if (layerIndex1 > 3 || layerIndex2 > 3) mat.DisableKeyword("_TERRAIN_BLEND_HEIGHT");
						}

						if ((mat.shader.name != DiffuseObjectShaderName) && mat.GetFloat("_DisableTerrainParallax") == 1)
						{
							mat.DisableKeyword("_TERRAIN_PARALLAX");
						}

						if (mat.GetFloat("_DisableDistanceBlending") == 1)
						{
							mat.DisableKeyword("_TERRAIN_DISTANCEBLEND");
						}

						if (hasNormalMap) { mat.EnableKeyword("_NORMALMAP"); } else { mat.DisableKeyword("_NORMALMAP"); }

						if (mat.shader.name == DiffuseObjectShaderName)
						{
							if (mat.GetTexture("_BumpMap")) { mat.EnableKeyword("_OBJECT_NORMALMAP"); } else { mat.DisableKeyword("_OBJECT_NORMALMAP"); }
						}
					}
				}
						
				#if UNITY_EDITOR
				//------ Checking Shader Variant Limit ------
				if (GraphicsSettings.renderPipelineAsset)
				{
					string warningText01 = "InTerra \"Object into Terrain Integration\" shader require the Shader Variant Limit to be at least ";
					string warningText02 = "There might be need to reimport the shader after the value change.";

					string pipeline = GraphicsSettings.renderPipelineAsset.GetType().Name;

					#if UNITY_2021_3_OR_NEWER
					if (EditorPrefs.GetInt("UnityEditor.ShaderGraph.VariantLimit", 0) < 1538 && !shaderVariantWarning)
					{
						EditorUtility.DisplayDialog("InTerra", warningText01 + "1538. \n\n " + warningText02, "Ok");
						shaderVariantWarning = true;
					}
					#else
						if (pipeline == "HDRenderPipelineAsset" && EditorPrefs.GetInt("UnityEditor.ShaderGraph.VariantLimit", 0) < 1538 && !shaderVariantWarning)
						{
							EditorUtility.DisplayDialog("InTerra", warningText01 + "1538. \n\n " + warningText02, "Ok");
							shaderVariantWarning = true;
						}

						if (pipeline == "UniversalRenderPipelineAsset" && EditorPrefs.GetInt("UnityEditor.ShaderGraph.VariantLimit", 0) < 1152 && !shaderVariantWarning)
						{
							EditorUtility.DisplayDialog("InTerra", warningText01 + "1152. \n\n " + warningText02, "Ok");
							shaderVariantWarning = true;
						}
					#endif
				}
				#endif				
			}
			TriplanarDataUpdate();
		}

		//============================================================================
		//-------------------------|		FUNCTIONS		|-------------------------
		//============================================================================
		public static bool CheckPosition(Terrain terrain, Vector2 position)
		{
			return terrain != null && terrain.terrainData != null
			&& terrain.GetPosition().x <= position.x && (terrain.GetPosition().x + terrain.terrainData.size.x) > position.x
			&& terrain.GetPosition().z <= position.y && (terrain.GetPosition().z + terrain.terrainData.size.z) > position.y;
		}

		public static bool CheckObjectShader(Material mat)
		{
			return mat && mat.shader && mat.shader.name != null 
			&& (mat.shader.name == ObjectShaderName
			 || mat.shader.name == DiffuseObjectShaderName 
			 || mat.shader.name == URPObjectShaderName
			 || mat.shader.name == HDRPObjectShaderName
			 || mat.shader.name == HDRPObjectTessellationShaderName);
		}

		public static bool CheckTerrainShader(Shader shader)
		{
			return shader.name == TerrainShaderName 
				|| shader.name == DiffuseTerrainShaderName 
				|| shader.name.Contains(URPTerrainShaderName)
				|| shader.name.Contains(HDRPTerrainShaderName)
				|| shader.name.Contains(HDRPTerrainTessellationShaderName);
		}

		#if UNITY_EDITOR
		public static TerrainLayer TerrainLayerFromGUID(Material mat, string tag)
			{
				return (TerrainLayer)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(mat.GetTag(tag, false)), typeof(TerrainLayer));
			}
		#endif

		public static void TerrainLaeyrDataToMaterial(TerrainLayer tl, int n, Material mat)
		{
			bool diffuse = mat.shader.name == DiffuseObjectShaderName;

			if (!diffuse)
			{
				#if UNITY_EDITOR
					if (tl)
					{
						TextureImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(tl.diffuseTexture)) as TextureImporter;
						if (importer && importer.DoesSourceTextureHaveAlpha())
						{
							tl.smoothness = 1;
						}
					}
				#endif				
				if (n < 4)
				{
					Vector4 smoothness = mat.GetVector("_TerrainSmoothness"); smoothness[n] = tl ? tl.smoothness : 0;
					Vector4 metallic = mat.GetVector("_TerrainMetallic"); metallic[n] = tl ? tl.metallic : 0;
					Vector4 normScale = mat.GetVector("_TerrainNormalScale"); normScale[n] = tl ? tl.normalScale : 1;
					mat.SetVector("_TerrainNormalScale", normScale);
					mat.SetVector("_TerrainSmoothness", smoothness);
					mat.SetVector("_TerrainMetallic", metallic);

				}
				else
				{
					Vector4 smoothness1 = mat.GetVector("_TerrainSmoothness1"); smoothness1[n - 4] = tl ? tl.smoothness : 0;
					Vector4 metallic1 = mat.GetVector("_TerrainMetallic1"); metallic1[n - 4] = tl ? tl.metallic : 0;
					Vector4 normScale1 = mat.GetVector("_TerrainNormalScale1"); normScale1[n - 4] = tl ? tl.normalScale : 1;
					mat.SetVector("_TerrainNormalScale1", normScale1);
					mat.SetVector("_TerrainSmoothness1", smoothness1);
					mat.SetVector("_TerrainMetallic1", metallic1);
				}
			}
			
			mat.SetTexture("_Splat" + n.ToString(), tl ? tl.diffuseTexture : null);
			mat.SetTexture("_Normal" + n.ToString(), tl ? tl.normalMapTexture : null);

			mat.SetTexture("_Mask" + n.ToString(), tl ? tl.maskMapTexture : null);
			mat.SetVector("_SplatUV" + n.ToString(), tl ? new Vector4(tl.tileSize.x, tl.tileSize.y, tl.tileOffset.x, tl.tileOffset.y) : new Vector4(1,1,0,0));
			mat.SetVector("_MaskMapRemapScale" + n.ToString(), tl ? tl.maskMapRemapMax - tl.maskMapRemapMin : new Vector4(1, 1, 1, 1));
			mat.SetVector("_MaskMapRemapOffset" + n.ToString(), tl ? tl.maskMapRemapMin : new Vector4(0, 0, 0, 0));
			mat.SetVector("_DiffuseRemapScale" + n.ToString(), tl ? tl.diffuseRemapMax : new Vector4(1, 1, 1, 1));
			mat.SetVector("_DiffuseRemapOffset" + n.ToString(), tl ? tl.diffuseRemapMin : new Vector4(0, 0, 0, 0));

			if(mat.HasProperty("_LayerHasMask"))
            {
				mat.SetFloat("_LayerHasMask" + n.ToString(), tl ? (float)(tl.maskMapTexture ? 1.0 : 0.0) : (float)0.0);
			}			
		}
		 
		public static void CheckLayerIndex(Terrain terrain, int n, Material mat, ref int layerIndex)
		{
			bool diffuse = mat.shader.name == DiffuseObjectShaderName;
			foreach (TerrainLayer tl in terrain.terrainData.terrainLayers)
			{
				bool equal = tl && mat.GetTexture("_Splat" + n.ToString()) == tl.diffuseTexture
				&& mat.GetTexture("_Normal" + n.ToString()) == tl.normalMapTexture
				&& mat.GetVector("_TerrainNormalScale")[n] == tl.normalScale
				&& mat.GetTexture("_Mask" + n.ToString()) == tl.maskMapTexture
				&& mat.GetVector("_SplatUV" + n.ToString()) == new Vector4(tl.tileSize.x, tl.tileSize.y, tl.tileOffset.x, tl.tileOffset.y)
				&& mat.GetVector("_MaskMapRemapScale" + n.ToString()) == tl.maskMapRemapMax - tl.maskMapRemapMin
				&& mat.GetVector("_MaskMapRemapOffset" + n.ToString()) == tl.maskMapRemapMin
				&& mat.GetVector("_DiffuseRemapScale" + n.ToString()) == tl.diffuseRemapMax
				&& mat.GetVector("_DiffuseRemapOffset" + n.ToString()) == tl.diffuseRemapMin;

				bool equalMetallicSmooth = diffuse || tl && mat.GetVector("_TerrainMetallic")[n] == tl.metallic
				&& mat.GetVector("_TerrainSmoothness")[n] == tl.smoothness;

				if (equal && equalMetallicSmooth)
				{
					layerIndex = terrain.terrainData.terrainLayers.ToList().IndexOf(tl);
					mat.SetFloat("_LayerIndex" + (n + 1).ToString(), layerIndex);
				}
			}
		}

		static void SetTerrainFloatsToMaterial(Terrain terrain, Material mat, string[] properties)
		{
			foreach (string prop in properties)
			{
				mat.SetFloat(prop, terrain.materialTemplate.GetFloat(prop));
			}
		}

		static void SetTerrainVectorToMaterial(Terrain terrain, Material mat, string value)
		{
			mat.SetVector(value, terrain.materialTemplate.GetVector(value));
		}

		static void SetTerrainTextureToMaterial(Terrain terrain, Material mat, string texture)
		{
			mat.SetTexture(texture, terrain.materialTemplate.GetTexture(texture));
			mat.SetTextureScale(texture, terrain.materialTemplate.GetTextureScale(texture));
			mat.SetTextureOffset(texture, terrain.materialTemplate.GetTextureOffset(texture));
		}

		static void TerrainKeywordsToMaterial(Terrain terrain, Material mat,  string[] keywords)
		{
			foreach (string keyword in keywords)
			{
				if (terrain.materialTemplate.IsKeywordEnabled(keyword))
				{
					mat.EnableKeyword(keyword);
				}
				else
				{
					mat.DisableKeyword(keyword);
				}
			}
		}

		static void DisableKeywords(Material mat, string[] keywords)
		{
			foreach (string keyword in keywords)
			{
				mat.DisableKeyword(keyword);
			}
		}
		public static InTerra_UpdateAndCheck GetUpdaterScript()
		{
			if (updateScript == null)
			{
				if (!updater)
				{
					if (!GameObject.Find(UpdaterName))
					{
						updater = new GameObject(UpdaterName);
						updater.AddComponent<InTerra_UpdateAndCheck>();

						updater.hideFlags = HideFlags.HideInInspector;
						updater.hideFlags = HideFlags.HideInHierarchy;
					}
					else
					{
						updater = GameObject.Find(UpdaterName);
					}
				}

				updateScript = updater.GetComponent<InTerra_UpdateAndCheck>();
			}

			return (updateScript);
		}

		public static void CheckAndUpdate()
		{
			if (Terrain.activeTerrains.Length > 0)
			{ 
				updateScript = GetUpdaterScript();
				DictionaryMaterialTerrain materialTerrain = updateScript.MaterialTerrain;

				if (materialTerrain != null && materialTerrain.Count > 0)
				{
					Material mat = materialTerrain.Keys.First();

					if (mat && materialTerrain[mat] && !mat.GetTexture("_TerrainHeightmapTexture") && materialTerrain[mat].terrainData.heightmapTexture.IsCreated())
					{
						UpdateTerrainData(InTerra_Setting.DictionaryUpdate);
					}
				}
				else if(!updateScript.FirstInit)
				{
					if (!InTerra_Setting.DisableAllAutoUpdates) UpdateTerrainData(true);
					updateScript.FirstInit = true;
				}

				#if UNITY_EDITOR
					TriplanarDataUpdate();
				#endif
			}
		}

		static void TriplanarDataUpdate()
		{
			Terrain[] terrains = Terrain.activeTerrains;
			foreach (Terrain terrain in terrains)
			{
				if (terrain && terrain.terrainData && terrain.materialTemplate && terrain.materialTemplate.IsKeywordEnabled("_TERRAIN_TRIPLANAR"))
				{
					terrain.materialTemplate.SetVector("_TerrainSizeXZPosY", new Vector3(terrain.terrainData.size.x, terrain.terrainData.size.z, terrain.transform.position.y));
				} 
			}
		}

		#if UNITY_EDITOR
			public static void CenterOnMainWin(this UnityEditor.EditorWindow aWin)
			{
				var main = EditorGUIUtility.GetMainWindowPosition();
				var pos = aWin.position;
				float w = (main.width - pos.width) * 0.5f;
				float h = (main.height - pos.height) * 0.5f;
				pos.x = main.x + w;
				pos.y = main.y + h;
				aWin.position = pos;
			}
		#endif
	}

	//The Serialized Dictionary is based on christophfranke123 code from this page https://answers.unity.com/questions/460727/how-to-serialize-dictionary-with-unity-serializati.html
	[System.Serializable]
	public class SerializableDictionary<TKey, TValue> : Dictionary<TKey, TValue>, ISerializationCallbackReceiver
	{
		[SerializeField]
		private List<TKey> keys = new List<TKey>();

		[SerializeField]
		private List<TValue> values = new List<TValue>();

		// save the dictionary to lists
		public void OnBeforeSerialize()
		{
			keys.Clear();
			values.Clear();
			foreach (KeyValuePair<TKey, TValue> pair in this)
			{
				keys.Add(pair.Key);
				values.Add(pair.Value);
			}
		}

		// load dictionary from lists
		public void OnAfterDeserialize()
		{
			this.Clear();
			if (keys.Count != values.Count)
				throw new System.Exception(string.Format("there are {0} keys and {1} values after deserialization. Make sure that both key and value types are serializable."));

			for (int i = 0; i < keys.Count; i++)
				this.Add(keys[i], values[i]);
		}

	}
}
