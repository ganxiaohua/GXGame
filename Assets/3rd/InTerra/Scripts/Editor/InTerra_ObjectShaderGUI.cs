using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;
using System.IO;

namespace InTerra
{
	public class InTerra_ObjectShaderGUI : ShaderGUI
	{	
		bool terrainInfo = false;
		bool objectInfo = false;
		bool objectInfoInit = false;
		bool minmax1 = false;
		bool minmax2 = false;
		bool minmaxNi = false;
		bool minmaxMip = false;
		bool nIntersect = false;
		bool tessDistances = false;

		bool multipleTerrainsMats = false;
		bool multipleTerrainsTags = false;
		bool terrainsDuplicitName = false;
		bool terrainsInvalidChars = false;
		bool matContainsInstance = false;
		bool matListInit = false;
		bool changingTerrainTag = false;

		MaterialProperty[] properties;
		Vector2 ScrollPos;
		Vector2 ScrollPosMats;

		Terrain terrain = null;
		TerrainLayer[] tLayers = null;
		bool isOnTerrain = false;

		List<Renderer> okTerrain = new List<Renderer>();
		List<Renderer> noTerrain = new List<Renderer>();
		List<Renderer> wrongTerrain = new List<Renderer>();

		Dictionary<Renderer, Terrain> rendTerrain = new Dictionary<Renderer, Terrain>();
		List<Material> relatedMaterials = new List<Material>();

		string baseName = "";
		Terrain terTag;

		enum NumberOfLayers
		{
			[Description("One Pass")] OnePass,
			[Description("One Layer")] OneLayer,
			[Description("Two Layers")] TwoLayers
		}

		enum TessellationMode
		{
			[Description("None")] None,
			[Description("Phong")] Phong
		}


		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			this.properties = properties;
			Material targetMat = materialEditor.target as Material;
			Rect textureRect;
			bool disableUpdates = InTerra_Setting.DisableAllAutoUpdates;
			bool updateDict = InTerra_Setting.DictionaryUpdate;
			bool updateAtOpen = InTerra_Setting.ObjectGUICheckAndUpdateAtOpen;

			//-------------------------- FONT STYLES -------------------------------
			var styleButtonBold = new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold };
			var styleBoldLeft = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleLeft };
			var styleLeft = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleLeft };
			var styBoldCenter = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
			var styleMiniBold = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleLeft, fontStyle = FontStyle.Bold };
			var styleMiniRight = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleRight }; 
			var styleMini = new GUIStyle(EditorStyles.miniLabel);
			var wrongStyleMini = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleLeft };
			wrongStyleMini.normal.textColor = Color.red;
			wrongStyleMini.hover.textColor = Color.red;


			//===================== TERRAIN & OBJECTS DATA =========================
			DictionaryMaterialTerrain materialTerrain = InTerra_Data.GetUpdaterScript().MaterialTerrain;
			Terrain[] terrains = Terrain.activeTerrains;

			if (terrains.Length == 0)
			{
				EditorGUILayout.HelpBox("There is no Terrain in this Scene!", MessageType.Warning);
				GUI.enabled = false;
			}
			else
			{
				if (updateAtOpen)
				{
					if (!objectInfoInit)
					{
						if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
						if (materialTerrain.ContainsKey(targetMat))
						{
							GetTerrain();
							CreateObjectsLists(targetMat, terrain);
						}
						objectInfoInit = true;
					}
					WrongTerrainWarning(terrain);
				}

				if (materialTerrain.ContainsKey(targetMat))
				{
					if (materialTerrain[targetMat] == null)
					{
						EditorGUILayout.HelpBox("The avarge position of the Objects using this Material is outside of any Terrain!", MessageType.Warning);
						GUI.enabled = false;
					}
					else
					{
						GetTerrain();
					}
				}
				else
				{
					EditorGUILayout.HelpBox("There are no Terrain Data for this Material or this Material is not assigned to any Object in this Scene!", MessageType.Warning);
					GUI.enabled = false;
				}
			}

			//=======================================================================
			//----------------------|   OBJECT TEXTURES    |-------------------------
			//=======================================================================
			string mainTex = "_MainTex";
			string mainColor = "_Color";
			string mainNormal = "_BumpMap";
			string mainMask = "_MainMask";
			string normalScale = "_BumpScale";
			string detailTex = "_DetailAlbedoMap";

			if (targetMat.shader.name == InTerra_Data.URPObjectShaderName)
			{
				mainTex = "_BaseMap";
				mainColor = "_BaseColor";
				mainNormal = "_BumpMap";
			}
			if (targetMat.shader.name.Contains("InTerra/HDRP"))
			{
				mainTex = "_BaseColorMap";
				mainColor = "_BaseColor";
				mainNormal = "_NormalMap";
				normalScale = "_NormalScale";
				detailTex = "_DetailMap";
				mainMask = "_MaskMap";
			}

			//------------------------ ALBEDO ----------------------------
			TextureSingleLine(mainTex, mainColor, "Albedo", "Albedo(RGB)");

			if (targetMat.shader.name == InTerra_Data.DiffuseObjectShaderName)
			{
				TextureImporter importer = AssetImporter.GetAtPath(AssetDatabase.GetAssetPath(FindProperty("_MainTex").textureValue)) as TextureImporter;
				if (importer && importer.DoesSourceTextureHaveAlpha())
				{
					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						EditorGUILayout.LabelField("Channel Remapping:");
						Vector4 offset = targetMat.GetVector("_MaskMapRemapOffset");
						Vector4 scale = targetMat.GetVector("_MaskMapRemapScale");
						RemapMask(ref offset.z, ref scale.z, "A: Heightmap", "Remap Heightmap in Albedo Alpha Channel");
						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra Object Channel Remapping");
							targetMat.SetVector("_MaskMapRemapOffset", offset);
							targetMat.SetVector("_MaskMapRemapScale", scale);
						}
					}
					EditorGUILayout.Space();
				}
			}

			//------------------------ NORMAL MAP ------------------------
			EditorGUI.BeginChangeCheck();
			TextureSingleLine(mainNormal, normalScale, "Normal Map", "Normal map");
			if (EditorGUI.EndChangeCheck() && targetMat.shader.name == InTerra_Data.DiffuseObjectShaderName)
			{
				materialEditor.RegisterPropertyChangeUndo("InTerra Object Normal Texture Keywodr");
				SetKeyword("_OBJECT_NORMALMAP", FindProperty(mainNormal).textureValue != null);
			}
			
			//------------------------ MASK MAP --------------------------
			if (targetMat.shader.name != InTerra_Data.DiffuseObjectShaderName)
			{ 
				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{
					EditorGUI.BeginChangeCheck();
					MaterialProperty maskTex = FindProperty(mainMask);
					targetMat.SetFloat("_HasMask", maskTex.textureValue ? 1.0f : 0.0f);

					using (new GUILayout.HorizontalScope())
					{
						textureRect = EditorGUILayout.GetControlRect(GUILayout.MinWidth(50));
						materialEditor.TexturePropertyMiniThumbnail(textureRect, maskTex, "Mask Map", "Mask Map Channels: \n R:Metallic \n G:A.Occlusion  \n B:Heightmap \n A:Smoothness ");
						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra Object Mask");
							targetMat.SetFloat("_HasMask", maskTex.textureValue ? 1.0f : 0.0f);						
						}
						if (GUILayout.Button(LabelAndTooltip("Mask Map Creator", "Open window for creating Mask Map")))
						{
							InTerra_MaskCreator.OpenWindow(false);
						}
					}

					EditorGUILayout.Space();
					if (maskTex.textureValue == null)
					{
						PropertyLine("_Metallic", "Metallic");
						if (targetMat.shader.name == InTerra_Data.URPObjectShaderName || targetMat.shader.name.Contains("InTerra/HDRP"))
						{
							PropertyLine("_Smoothness", "Smoothness");
						}
						else
						{
							PropertyLine("_Glossiness", "Smoothness");
						}
						

						using (new GUILayout.HorizontalScope())
						{
							GUILayout.Label("A. Occlusion", GUILayout.MinWidth(118));
							float ao = targetMat.GetFloat("_Ao");

							EditorGUI.BeginChangeCheck();

							ao = EditorGUILayout.Slider(1 - ao, 0, 1);

							if (EditorGUI.EndChangeCheck())
							{
								materialEditor.RegisterPropertyChangeUndo("InTerra Object AO");
								targetMat.SetFloat("_Ao", 1 - ao);
							}
						}
					}
					else
					{				
						EditorGUILayout.LabelField("Channels Remapping:");
						EditorGUILayout.Space();

						Vector4 offset = targetMat.GetVector("_MaskMapRemapOffset");
						Vector4 scale = targetMat.GetVector("_MaskMapRemapScale");

						EditorGUI.BeginChangeCheck();					
						RemapMask(ref offset.x, ref scale.x, "R: Metallic", "Remap Metallic Map in Red Channel");
						RemapMask(ref offset.y, ref scale.y, "G: A. Occlusion", "Remap A. Occlusion Map in Green Channel");
						RemapMask(ref offset.z, ref scale.z, "B: Heightmap", "Remap Heightmap in Blue Channel");
						RemapMask(ref offset.w, ref scale.w, "A: Smoothness", "Remap Smoothness Map in Alpha Channel");

						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra Object Channel Remapping");
							targetMat.SetVector("_MaskMapRemapOffset", offset);
							targetMat.SetVector("_MaskMapRemapScale", scale);
						}
					}		
				}
			}
			Texture mainMaskTexture = targetMat.GetTexture(mainMask);

			//--------------------- TEXTURES PROPERTY -------------------------
			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{				
				materialEditor.TextureScaleOffsetProperty(FindProperty(mainTex));
			}
			EditorGUILayout.Space();

			//------------------------------ PARALAX -------------------------------
			if (targetMat.shader.name != InTerra_Data.DiffuseObjectShaderName && !targetMat.shader.name.Contains(InTerra_Data.TessellationShaderFolder))
			{
				bool parallax = targetMat.IsKeywordEnabled("_OBJECT_PARALLAX");

				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{
					EditorStyles.label.fontStyle = FontStyle.Bold;
					EditorGUI.BeginChangeCheck();
					parallax = EditorGUILayout.ToggleLeft(LabelAndTooltip("Parallax Occlusion Mapping", "An illusion of 3D effect created by offsetting the texture depending on heightmap."), parallax);
					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Object Parallax");
						SetKeyword("_OBJECT_PARALLAX", parallax);
					}
					EditorStyles.label.fontStyle = FontStyle.Normal;

					if (parallax)
					{						
						if (mainMaskTexture != null) targetMat.SetFloat("_MipMapCount", mainMaskTexture.mipmapCount);

						using (new GUILayout.VerticalScope(EditorStyles.helpBox))
						{

							float affineSteps = targetMat.GetFloat("_ParallaxAffineSteps");
							float parallaxHeight = targetMat.GetFloat("_ParallaxHeight");
							float parallaxSteps = targetMat.GetFloat("_ParallaxSteps");

							EditorGUI.BeginChangeCheck();
							using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
							{							
								GUILayout.Label(LabelAndTooltip("Affine Steps: ", "The higher number the smoother transition between steps, but also the higher number will increase performance heaviness."));
								affineSteps = EditorGUILayout.IntField((int)affineSteps, GUILayout.MaxWidth(60));
								affineSteps = Mathf.Clamp(affineSteps, 1, 10);								
							}

							GUILayout.Label(LabelAndTooltip("Steps", "Each step is creating a new layer for offsetting. The more steps, the more precise the parallax effect will be, but also the higher number will increase performance heaviness."), styleMiniRight);
							using (new GUILayout.HorizontalScope())
							{
								parallaxHeight = EditorGUILayout.FloatField(LabelAndTooltip("Height Amplitude:", "The value of the height illusion."), parallaxHeight, GUILayout.MinWidth(60));
								parallaxHeight = Mathf.Clamp(parallaxHeight, 0, 15);
															
								parallaxSteps = EditorGUILayout.IntField((int)parallaxSteps, GUILayout.MaxWidth(30));
								parallaxSteps = Mathf.Clamp(parallaxSteps, 0, 50);															
							}
							if (EditorGUI.EndChangeCheck())
							{
								materialEditor.RegisterPropertyChangeUndo("InTerra Object Parallax Values");
								targetMat.SetFloat("_ParallaxAffineSteps", affineSteps);
								targetMat.SetFloat("_ParallaxHeight", parallaxHeight);
								targetMat.SetFloat("_ParallaxSteps", parallaxSteps);
							}

							EditorGUILayout.Space();

							if (terrain != null && terrain.materialTemplate.IsKeywordEnabled("_TERRAIN_PARALLAX"))
							{
								EditorGUILayout.HelpBox("The setting of Mip Maps fading distance is synchronized with the Terrain setting.", MessageType.Info);
							}
							else
							{
								using (new GUILayout.VerticalScope(EditorStyles.helpBox))
								{
									InTerra_GUI.MipMapsFading(targetMat, "Mip Maps Fading", materialEditor, ref minmaxMip);
								}
							}
						}
					}					
				}
			}

			//------------------------- DETAIL -------------------------
			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorStyles.label.fontStyle = FontStyle.Bold;
				bool detail = targetMat.IsKeywordEnabled("_OBJECT_DETAIL");

				EditorGUI.BeginChangeCheck();

				detail = EditorGUILayout.ToggleLeft(LabelAndTooltip("Detail Map", "Secondary textures"), detail);
				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo("InTerra Object DetailMap");
					SetKeyword("_OBJECT_DETAIL", detail);
				}

				EditorStyles.label.fontStyle = FontStyle.Normal;

				if (detail)
				{
					materialEditor.TexturePropertySingleLine(new GUIContent("Detail Albedo"), FindProperty(detailTex));
					targetMat.SetFloat("_HasDetailAlbedo", FindProperty(detailTex).textureValue ? 1.0f : 0.0f);
					TextureSingleLine("_DetailNormalMap", "_DetailNormalMapScale", "Normal Map", "Detail Normal Map");

					materialEditor.ShaderProperty(FindProperty("_DetailStrenght"), new GUIContent("Detail Strenght"));
					targetMat.SetFloat("_DetailNormalStrenght", targetMat.GetFloat("_DetailStrenght"));

					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						materialEditor.TextureScaleOffsetProperty(FindProperty(detailTex));
					}
				}
			}

			//-------------------------  EMISSION  -------------------------------
			if (targetMat.HasProperty("_EmissionEnabled"))
			{
				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{

					bool emission = targetMat.GetFloat("_EmissionEnabled") == 1;

					EditorStyles.label.fontStyle = FontStyle.Bold;
					EditorGUI.BeginChangeCheck();
					emission = EditorGUILayout.ToggleLeft(LabelAndTooltip("Emission", "Light Emission of Object."), emission);
					EditorStyles.label.fontStyle = FontStyle.Normal;

					if (emission)
					{
						string emissionMap = "_EmissionMap";
						string emissionColor = "_EmissionColor";

						if (targetMat.shader.name.Contains("InTerra/HDRP"))
						{
							emissionMap = "_EmissiveColorMap";
							emissionColor = "_EmissiveColor";
						}

						using (new GUILayout.VerticalScope(EditorStyles.helpBox))
						{
							materialEditor.TexturePropertyWithHDRColor(new GUIContent() { text = "Emission Map", tooltip = "Specifies the emissive color (RGB) of the Material." }, FindProperty(emissionMap), FindProperty(emissionColor), false);
							materialEditor.TextureScaleOffsetProperty(FindProperty(emissionMap));
						}

						if(targetMat.HasProperty("_EmissiveExposureWeight"))
						{
							using (new GUILayout.VerticalScope(EditorStyles.helpBox))
							{
								PropertyLine("_EmissiveExposureWeight", "Exposure weight", "Controls how the camera exposure influences the perceived intensity of the emissivity. A weight of 0 means that the emissive intensity is calculated ignoring the exposure; increasing this weight progressively increases the influence of exposure on the final emissive value.");
							}
						}
						using (new GUILayout.VerticalScope(EditorStyles.helpBox))
						{
							materialEditor.LightmapEmissionFlagsProperty(0, emission);
						}
					}

					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Emission");
						if (emission) targetMat.SetFloat("_EmissionEnabled", 1); else targetMat.SetFloat("_EmissionEnabled", 0);
					}
				}
			}
			EditorGUILayout.Space();


			//================================================================
			//-------------------|   TERRAIN LAYERS    |----------------------
			//================================================================
			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("TERRAIN LAYERS", styBoldCenter);

				NumberOfLayers layers = NumberOfLayers.OnePass;
				if (targetMat.IsKeywordEnabled("_LAYERS_ONE"))
				{
					layers = NumberOfLayers.OneLayer;
				}
				else if (targetMat.IsKeywordEnabled("_LAYERS_TWO"))
				{
					layers = NumberOfLayers.TwoLayers;
				}
				EditorGUI.BeginChangeCheck();
				layers = (NumberOfLayers)EditorGUILayout.EnumPopup(layers);
				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo("InTerra Shader Variant");
					SetKeyword("_LAYERS_ONE", layers == NumberOfLayers.OneLayer);
					SetKeyword("_LAYERS_TWO", layers == NumberOfLayers.TwoLayers);
					if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
				}

				//----------------------	ONE LAYER   ----------------------
				if (isOnTerrain && layers == NumberOfLayers.OneLayer)
				{
					SelectTerrainLayer(1, "Terrain Layer:");
				}

				//----------------------   TWO LAYERS   -----------------------
				if (isOnTerrain && layers == NumberOfLayers.TwoLayers)
				{
					SelectTerrainLayer(1, "Terrain Layer 1:");
					SelectTerrainLayer(2, "Terrain Layer 2:");
				}

				//----------------------   ONE PASS   -----------------------
				if (isOnTerrain && layers == NumberOfLayers.OnePass)
				{
					List<string> passes = new List<string>();
					int layersInPass = targetMat.HasProperty("_Control1") ? 8 : 4;					
					int passNumber = (int)targetMat.GetFloat("_PassNumber");
					int passesList = passNumber + 1;
					if (terrain.terrainData.alphamapTextureCount <= passNumber)
					{
						EditorGUILayout.HelpBox("The Terrain do not have pass " + ( passNumber + 1 ) + ".", MessageType.Warning);
					}
					else
					{
						passesList = (int) Mathf.Ceil((float)terrain.terrainData.alphamapLayers / layersInPass );
					}
					
					for (int i = 0; i < (passesList) ; i++)
					{
						passes.Add("Pass " + (i + 1).ToString() + " - Layers  "  + (i * layersInPass + 1).ToString() + " - " + (i * layersInPass + layersInPass).ToString());
					}

					if (!targetMat.shader.name.Contains("InTerra/HDRP"))
					{
						EditorGUI.BeginChangeCheck();
						passNumber = EditorGUILayout.Popup(passNumber, passes.ToArray(), GUILayout.MinWidth(150));

						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra LayerNumber1");
							targetMat.SetFloat("_PassNumber", passNumber);
							if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
						}
					}

					GUILayout.BeginHorizontal();
					for (int i = passNumber * layersInPass; i < (passNumber * layersInPass + layersInPass); i++)
					{
						string layerName = "Empty";
						Texture2D layerTexture = null;

						if (i < terrain.terrainData.alphamapLayers)
						{
							TerrainLayer tl = tLayers[i];
							if (tl)
							{
								layerName = tl.name;
								layerTexture = AssetPreview.GetAssetPreview(tl.diffuseTexture);
							}
							else
							{
								layerName = "Missing";
							}
						}
						if (i < terrain.terrainData.alphamapLayers)
						{
							using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(50)))
							{
								if (layerTexture)
								{
									GUI.DrawTexture(EditorGUILayout.GetControlRect(GUILayout.Width(48), GUILayout.Height(48)), layerTexture, ScaleMode.ScaleAndCrop);
								}
								else
								{
									EditorGUILayout.GetControlRect(GUILayout.Width(48), GUILayout.Height(48));
								}
								EditorGUILayout.LabelField(layerName, styleMini, GUILayout.Width(48), GUILayout.Height(12));
							}
						}

					}
					GUILayout.EndHorizontal();
				}
			}

			//============================================================================
			//-----------------------|  TERRAIN INTERSECTION  |---------------------------
			//============================================================================
			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("TERRAIN INTERSECTION", styBoldCenter);
				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{
					EditorGUI.BeginChangeCheck();

					Vector4 intersection = InTerra_GUI.MinMaxValues(targetMat.GetVector("_Intersection"), false, ref minmax1);

					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra BlendingIntersection");
						targetMat.SetVector("_Intersection", intersection);
					}

					EditorGUILayout.Space();

					PropertyLine("_Sharpness", "Sharpness", "Sharpness of blending");

					EditorGUI.BeginChangeCheck();

					EditorGUI.indentLevel = 1;
					nIntersect = EditorGUILayout.Foldout(nIntersect, LabelAndTooltip("Mesh Normals Intersection", "The height of intersection of terrain's and object's mesh normals. This value is calculated per vertex and it always affects the whole polygon!"), true);
					EditorGUI.indentLevel = 0;
					if (nIntersect)
					{
						Vector4 normalIntersect = InTerra_GUI.MinMaxValues(targetMat.GetVector("_NormIntersect"), false, ref minmaxNi);

						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra NormalIntersection");
							targetMat.SetVector("_NormIntersect", normalIntersect);
						}

					}
				}


				//============================= STEEP SLOPES =============================
				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{
					EditorGUILayout.LabelField("Steep slopes", styBoldCenter);

					//------------------------- SECONDARY INTERSECTION  -------------------------------
					bool steepIntersect = targetMat.GetFloat("_SteepIntersection") == 1; 

					EditorGUI.BeginChangeCheck();
					steepIntersect = EditorGUILayout.ToggleLeft(LabelAndTooltip("Secondary Intersection", "Separated intersection for steep slopes."), steepIntersect);
					Vector4 intersection2 = targetMat.GetVector("_Intersection2");

					if (steepIntersect)
					{
						intersection2 = InTerra_GUI.MinMaxValues(intersection2, false, ref minmax2);
						PropertyLine("_Steepness", "Steepness adjust", "This value adjusts the angle that will be considered as steep.");
					}
					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Secondary Intersection");
						SetKeyword("_OBJECT_STEEP_INTERSECTION", steepIntersect);
						if (steepIntersect) targetMat.SetFloat("_SteepIntersection", 1); else targetMat.SetFloat("_SteepIntersection", 0);
						targetMat.SetVector("_Intersection2", intersection2);
					}

					//------------------------------ TRIPLANAR -------------------------------
					bool triplanar = targetMat.IsKeywordEnabled("_OBJECT_TRIPLANAR");
					bool disOffset = targetMat.GetFloat("_DisableOffsetY") == 1;

					EditorGUI.BeginChangeCheck();
					triplanar = EditorGUILayout.ToggleLeft(LabelAndTooltip("Triplanar Mapping", "The Texture on steep slopes of Object will not be stretched."), triplanar);

					if (triplanar)
					{
						EditorGUI.indentLevel = 1;
						EditorStyles.label.fontSize = 10;
						disOffset = EditorGUILayout.ToggleLeft(LabelAndTooltip("Disable Height and Position Offset", "Front and Side projection of texture is offsetting by position and height to fit the Terrain texture as much as possible, but in some cases, if there is too steep slope of terrain, it can get stretched and it is better to disable the offsetting."), disOffset, GUILayout.Width(200));
						EditorStyles.label.fontSize = 12;
						EditorGUI.indentLevel = 0;
					}
					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Object Triplanar");
						SetKeyword("_OBJECT_TRIPLANAR", triplanar);
						if (disOffset) targetMat.SetFloat("_DisableOffsetY", 1); else targetMat.SetFloat("_DisableOffsetY", 0);
					}

					//------------------------------ DISTORTION -------------------------------
					if (!triplanar)
					{
						EditorStyles.label.fontSize = 11;
						materialEditor.ShaderProperty(FindProperty("_SteepDistortion"), LabelAndTooltip("Distortion (by Albedo)", "This value distorts stretched texture on Steep slopes, this is useful if you don't want to use triplanar - which is more performance heavy. Distortion is calculated by Albedo Texture and doesn't work with a single color."));
						EditorStyles.label.fontSize = 12;
					}
				}

				//------------------------------ DISABLE HIDE TILING -------------------------------
				if (terrain != null && terrain.materialTemplate.IsKeywordEnabled("_TERRAIN_DISTANCEBLEND"))
				{
					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						bool distanceBlend = targetMat.GetFloat("_DisableDistanceBlending") == 1;

						EditorGUI.BeginChangeCheck();
						distanceBlend = EditorGUILayout.ToggleLeft(LabelAndTooltip("Disable Hide Tiling", "If Terrain \"Hide Tiling\" is set on, this option will turn it off only for this Material to prevent additional samplings and calculations. This may cause some more or less visible seams in distance."), distanceBlend);

						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra Disable Hide Tiling");

							if (distanceBlend)
							{
								targetMat.SetFloat("_DisableDistanceBlending", 1);
								targetMat.DisableKeyword("_TERRAIN_DISTANCEBLEND");
							}
							else
							{
								targetMat.SetFloat("_DisableDistanceBlending", 0);
								SetKeyword("_TERRAIN_DISTANCEBLEND", terrain.materialTemplate.IsKeywordEnabled("_TERRAIN_DISTANCEBLEND"));
							}
						}
					}
				}
			

				//------------------------------ DISABLE TERRAIN PARALLAX -------------------------------
				if (terrain != null && targetMat.shader.name != InTerra_Data.DiffuseObjectShaderName && terrain.materialTemplate.IsKeywordEnabled("_TERRAIN_PARALLAX"))
				{
					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						bool terrainParallax = targetMat.GetFloat("_DisableTerrainParallax") == 1;

						EditorGUI.BeginChangeCheck();
						terrainParallax = EditorGUILayout.ToggleLeft(LabelAndTooltip("Disable Terrain Parallax", "If Terrain \"Parallax Occlusion Mapping\" is set on, this option will turn it off only for this Material."), terrainParallax);

						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra Disable Terrain Parallax");

							if (terrainParallax)
							{
								targetMat.SetFloat("_DisableTerrainParallax", 1);
								targetMat.DisableKeyword("_TERRAIN_PARALLAX");
							}
							else
							{
								targetMat.SetFloat("_DisableTerrainParallax", 0);
								SetKeyword("_TERRAIN_PARALLAX", terrain.materialTemplate.IsKeywordEnabled("_TERRAIN_PARALLAX"));
							}
						}
					}
				}
			}

			//============================================================================
			//----------------------------|  TESSELLATION  |------------------------------
			//============================================================================
			if (targetMat.shader.name.Contains(InTerra_Data.HDRPObjectTessellationShaderName))
			{
				if (mainMaskTexture != null) targetMat.SetFloat("_MipMapCount", mainMaskTexture.mipmapCount);

				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{
					EditorGUILayout.LabelField("TESSELLATION", styBoldCenter);

					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						PropertyLine("_TessellationFactor", "Tessellation Factor", "Controls the strength of the tessellation effect. Higher values result in more tessellation. Maximum tessellation factor is 15 on the Xbox One and PS4");
						PropertyLine("_TessellationBackFaceCullEpsilon", "Triangle Culling Epsilon", "Controls triangle culling. A value of -1.0 disables back face culling for tessellation, higher values produce more aggressive culling and better performance.");
						PropertyLine("_TessellationFactorTriangleSize", "Triangle Size", "Sets the desired screen space size of triangles (in pixels). Smaller values result in smaller triangle. Set to 0 to disable adaptative factor with screen space size.");

						TessellationMode tessMode = targetMat.IsKeywordEnabled("_TESSELLATION_PHONG") ? TessellationMode.Phong : TessellationMode.None;

						EditorGUI.BeginChangeCheck();
						using (new GUILayout.HorizontalScope())
						{
							EditorGUILayout.LabelField(LabelAndTooltip("Tessellation Mode", "Specifies the method HDRP uses to tessellate the mesh. None uses only the Displacement Map to tessellate the mesh. Phong tessellation applies additional Phong tessellation interpolation for smoother mesh."), GUILayout.Width(120));
							tessMode = (TessellationMode)EditorGUILayout.EnumPopup(tessMode);
						}

						if (tessMode == TessellationMode.Phong)
						{
							PropertyLine("_TessellationShapeFactor", "Shape Factor", "Controls the strength of Phong tessellation shape (lerp factor).");
						}
						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra Object Tessellation Mode");
							SetKeyword("_TESSELLATION_PHONG", tessMode == TessellationMode.Phong);
						}

						if(terrain.materialTemplate.shader.name.Contains(InTerra_Data.TessellationShaderFolder))
                        {
							EditorGUILayout.HelpBox("The setting of distance fading and shadows quality is synchronized with the Terrain setting.", MessageType.Info);
						}
						else
                        {
							PropertyLine("_TessellationShadowQuality", "Shadows quality", "Setting of shadows accuracy calculation. Higher value means more precise calculation.");
							EditorGUI.indentLevel = 1;
							tessDistances = EditorGUILayout.Foldout(tessDistances, "Fading Distances", true);
							EditorGUI.indentLevel = 0;
							if(tessDistances)
                            {
								InTerra_GUI.TessellationDistaces(targetMat, materialEditor, ref minmaxMip);
							}														
						}						
					}

					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						EditorGUILayout.LabelField("Displacement", styleBoldLeft);

						float tessDisplacement = targetMat.GetFloat("_TessellationDisplacement") * 100;
						float tessOffset = targetMat.GetFloat("_TessellationOffset") * 100;
						float terrainTessOffset = targetMat.GetFloat("_TerrainTessOffset") * 100;

						EditorGUI.BeginChangeCheck();

						tessDisplacement = EditorGUILayout.FloatField(LabelAndTooltip("Amplitude", "Amplitude of the Height Map (Blue channel in Mask Map)."), tessDisplacement);
						tessOffset = EditorGUILayout.FloatField(LabelAndTooltip("Height Offset", "Height offset for displacement."), tessOffset);
						terrainTessOffset = EditorGUILayout.FloatField(LabelAndTooltip("Terrain Layers Offset", " Offset for Terrain Layers displacement."), terrainTessOffset);

						tessDisplacement = Mathf.Clamp(tessDisplacement, 0, 50) * 0.01f;
						tessOffset = Mathf.Clamp(tessOffset, -50, 50) * 0.01f;
						terrainTessOffset = Mathf.Clamp(terrainTessOffset, -50, 50) * 0.01f;

						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra Object Tessellation Properties");
							targetMat.SetFloat("_TessellationDisplacement", tessDisplacement);
							targetMat.SetFloat("_TessellationOffset", tessOffset);
							targetMat.SetFloat("_TerrainTessOffset", terrainTessOffset);

							float terrainMaxDisplacement = 0;
							if (terrain.materialTemplate.HasProperty("_TessellationMaxDisplacement"))
							{
								terrainMaxDisplacement = terrain.materialTemplate.GetFloat("_TessellationMaxDisplacement");
							}
							float objectMaxDisplacement = (tessDisplacement / 2) + tessOffset + terrainTessOffset;

							float maxDisplacement = objectMaxDisplacement > terrainMaxDisplacement ? objectMaxDisplacement : terrainMaxDisplacement;
							
							targetMat.SetFloat("_TessellationObjMaxDisplacement", objectMaxDisplacement);
							targetMat.SetFloat("_TessellationMaxDisplacement", maxDisplacement);
						}
					}
					PropertyLine("_Tessellation_Sharpness", "Blending Sharpness", "Heightmap blending sharpness between Terrains and Objects Textures for Tessellation.");
				}
			}


			//===========================================================================
			//-----------------------|  MULTIPLE TERRAINS MATERIALS  |-------------------
			//===========================================================================
			if (terrains.Length > 1)
			{
				baseName = targetMat.GetTag("BaseName", false);

				if (baseName.Length < 1)
				{
					baseName = targetMat.name;
				}

				using (new GUILayout.VerticalScope())
				{
					EditorGUI.indentLevel = 1;
					multipleTerrainsMats = EditorGUILayout.Foldout(multipleTerrainsMats, "Multiple Terrains Materials ", true);
					EditorGUI.indentLevel = 0;

					if (multipleTerrainsMats)
					{
						using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
						{
							EditorGUILayout.LabelField(EditorGUIUtility.IconContent("console.warnicon"), GUILayout.Height(14), GUILayout.Width(14));
							EditorGUILayout.LabelField("This feature is currently Beta!", styleMini);
							
						}
						if (!matListInit)
						{
							if (!objectInfoInit)
							{
								if (!disableUpdates) InTerra_Data.UpdateTerrainData(true);
								if (materialTerrain.ContainsKey(targetMat))
								{
									GetTerrain();
									CreateObjectsLists(targetMat, terrain);
								}
								objectInfoInit = true;
							}
							InTerra_MaterialManager.CreateRelatedMaterialsList(baseName, relatedMaterials);
							matListInit = true;
						}
						EditorGUILayout.Space();

						//---------------|  MATERIAL BASE NAME  |--------------------
						if (targetMat.GetTag("BaseName", false).Length > 0 )
						{
							if (wrongTerrain.Count == 0 && noTerrain.Count == 0)
							{
								InTerra_MaterialManager.MaterialNameWarnings(terrain, targetMat, baseName, matContainsInstance, terrainsDuplicitName, terrainsInvalidChars, rendTerrain, relatedMaterials);
								EditorGUILayout.Space();
							}
														
							GUILayout.Label("Base Name of Material : ");
							using (new GUILayout.VerticalScope(EditorStyles.helpBox))
							{
								GUILayout.Label(baseName, new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = 14 });	
							}
						}
						else
                        {
							using (new GUILayout.VerticalScope(EditorStyles.helpBox))
							{
								GUI.enabled = false;
								GUILayout.Label("---", new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter, fontSize = 14 });
							}
						}

						//---------------|  MATERIAL TAGS  |--------------------
						using (new GUILayout.VerticalScope(EditorStyles.helpBox))
						{										
							EditorGUI.indentLevel = 1; EditorStyles.foldout.fontSize = 10;
							multipleTerrainsTags = EditorGUILayout.Foldout(multipleTerrainsTags, "Material Tags", true);
							EditorGUI.indentLevel = 0; EditorStyles.foldout.fontSize = 12;

							if (multipleTerrainsTags)
							{
								InTerra_MaterialManager.MaterialTags(ref changingTerrainTag, targetMat, ref terTag, materialEditor, styleMini);
							}
						}
						EditorGUILayout.Space();

						//---------------|  RELATED MATERIALS LIST  |--------------------
						GUILayout.Label("Related Materials:", styleMini);
						using (new GUILayout.VerticalScope(EditorStyles.helpBox))
						{
							ScrollPosMats = EditorGUILayout.BeginScrollView(ScrollPosMats, GUILayout.Height(100));
							foreach (var mat in relatedMaterials)
							{
								if (mat.name != targetMat.name)
								{
									string labelInfo = InTerra_Data.CheckObjectShader(mat) ? "" : "\n\nThis Material does not have InTerra shader, copy of the properties will not be applied.";
									GUIStyle labelStyle = InTerra_Data.CheckObjectShader(mat) ? styleMini : wrongStyleMini;

									if (GUILayout.Button(LabelAndTooltip(mat.name, AssetDatabase.GetAssetPath(mat) + labelInfo), labelStyle))
									{
										EditorGUIUtility.PingObject(mat);
									}									
								}
							}
							EditorGUILayout.EndScrollView();
						}

						//---------------|   COPY PROPERTIES TO RELATED MATERIALS   |----------------------
						using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
						{
							if (GUILayout.Button("Copy Properties to Related Materials", styleButtonBold))
							{
								InTerra_MaterialManager.CopyPropertiesToRelated(targetMat, relatedMaterials);
							}
						}
						EditorGUILayout.Space();

						//---------------|   REASSIGN RELATED MATERIALS   |----------------------
						if (GUILayout.Button("Reassign Related Materials"))
						{
							InTerra_MaterialManager.ReassignMaterials(baseName, relatedMaterials, rendTerrain);

							if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
							GetTerrain();
							CreateObjectsLists(targetMat, terrain);
							InTerra_MaterialManager.CreateRelatedMaterialsList(baseName, relatedMaterials);
						}

						//---------------|   OPEN MATERIAL CREATOR WINDOW   |----------------------
						GUI.enabled = terrain && !matContainsInstance && !terrainsDuplicitName ;
						if (GUILayout.Button(LabelAndTooltip("Create, Rename, Delete...", "Open window for Material Creator")))
						{
							InTerra_MaterialCreator.OpenWindow(targetMat, rendTerrain, relatedMaterials, baseName.Length > 0 ? baseName : targetMat.name);
						}
						EditorGUILayout.Space();
					}
				}
				GUI.enabled = true;
			}
			

			//================= TERRAIN INFO ================
			EditorGUI.indentLevel = 1;
			terrainInfo = EditorGUILayout.Foldout(terrainInfo, "Terrain info", true);
			EditorGUI.indentLevel = 0;
			if (terrainInfo && isOnTerrain)
			{
				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{
					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Name:", styleBoldLeft, GUILayout.Width(60));
					EditorGUILayout.LabelField(terrain.name, styleLeft, GUILayout.MinWidth(50));
					GUILayout.EndHorizontal();

					GUILayout.BeginHorizontal();
					EditorGUILayout.LabelField("Position:", styleBoldLeft, GUILayout.Width(60));
					Vector3 tp = terrain.GetPosition();

					EditorGUILayout.LabelField("X: " + tp.x.ToString(), styleLeft, GUILayout.MinWidth(50));
					EditorGUILayout.LabelField("Y: " + tp.y.ToString(), styleLeft, GUILayout.MinWidth(50));
					EditorGUILayout.LabelField("Z: " + tp.z.ToString(), styleLeft, GUILayout.MinWidth(50));
					GUILayout.EndHorizontal();
				}
				EditorGUI.indentLevel = 0;
			}

			GUI.enabled = true;

			//================= OBJECT INFO ================
			EditorGUI.indentLevel = 1;
			objectInfo = EditorGUILayout.Foldout(objectInfo, "Objects info", true);
			EditorGUI.indentLevel = 0;
			if (objectInfo)
			{
				if (!objectInfoInit)
				{
					if (!disableUpdates) InTerra_Data.UpdateTerrainData(true);
					if (materialTerrain.ContainsKey(targetMat))
					{
						GetTerrain();
						CreateObjectsLists(targetMat, terrain);
					}
					objectInfoInit = true;
				}
				if (!updateAtOpen)
				{
					WrongTerrainWarning(terrain);
				}

				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{
					using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
					{					
						GUILayout.Label("Name", styleMiniBold, GUILayout.MinWidth(60));
						GUILayout.Label("position (x,y,z)", styleMiniBold, GUILayout.MinWidth(40));
						GUILayout.Label("Go to Object", styleMiniBold, GUILayout.Width(65));		
					}

					ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos, GUILayout.Height(100));
				
					ObjectsList(noTerrain, Color.red);
					ObjectsList(wrongTerrain, new Color(1.0f, 0.5f, 0.0f));
					ObjectsList(okTerrain, Color.black);

					EditorGUILayout.EndScrollView();
				}
				EditorGUI.indentLevel = 0;
			}
			GUI.enabled = true;

			//============= UPDATE TERRAIN DATA ==============
			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				if (GUILayout.Button("Update Terrain Data", styleButtonBold))
				{
					InTerra_Data.UpdateTerrainData(true);

					if (terrains.Length > 0 && materialTerrain.ContainsKey(targetMat))
					{
						GetTerrain();
						CreateObjectsLists(targetMat, terrain);
						InTerra_MaterialManager.CreateRelatedMaterialsList(baseName, relatedMaterials);
					}					
				}
			}

			EditorGUILayout.Space();
			EditorGUILayout.Space();

			//-------------------------------------------------------------
			materialEditor.RenderQueueField();
			materialEditor.EnableInstancingField();
			materialEditor.DoubleSidedGIField();

			if (targetMat.shader.name.Contains("InTerra/HDRP"))
			{
				GUI.enabled = false;
				targetMat.SetFloat("_AddPrecomputedVelocity", 0.0f);
				PropertyLine("_AddPrecomputedVelocity", "Add Precomputed Velocity", "Requires additional per vertex velocity info.");
			}
			//-------------------------------------------------------------


			//========================================================================
			//---------------------------|   WARNINGS   |-----------------------------
			//========================================================================
			void WrongTerrainWarning(Terrain terrain)
			{
				if (terrain != null)
				{
					if (noTerrain.Count > 0 && noTerrain.Count < 2)
					{
						EditorGUILayout.HelpBox("The Object " + noTerrain[0].name + " with this material is outside of any Terrain!", MessageType.Warning);
					}

					if (noTerrain.Count > 1)
					{
						EditorGUILayout.HelpBox("Some Objects with this material are outside of any Terrain!", MessageType.Warning);
					}

					if (wrongTerrain.Count > 0 && wrongTerrain.Count < 2)
					{
						EditorGUILayout.HelpBox("The Object " + wrongTerrain[0].name + " with this material is not on correct Terrain!", MessageType.Warning);
					}

					if (wrongTerrain.Count > 1)
					{
						EditorGUILayout.HelpBox("Some Objects with this material are not on correct Terrain!", MessageType.Warning);
					}
				}
			}

			//=====================================================================================
			//=====================================================================================

			void PropertyLine(string property, string label, string tooltip = null)
			{
				materialEditor.ShaderProperty(FindProperty(property), new GUIContent() { text = label, tooltip = tooltip });
			}

			void TextureSingleLine(string property1, string property2, string label, string tooltip = null)
			{
				materialEditor.TexturePropertySingleLine(new GUIContent() { text = label, tooltip = tooltip }, FindProperty(property1), FindProperty(property2) );
			}

			GUIContent LabelAndTooltip(string label, string tooltip)
			{
				return new GUIContent() { text = label, tooltip = tooltip };
			}

			void SetKeyword(string name, bool set)
			{
				if (set) targetMat.EnableKeyword(name); else targetMat.DisableKeyword(name);
			}

			void RemapMask(ref float offset, ref float scale, string label, string tooltip = null)
			{
				using (new GUILayout.HorizontalScope())
				{
					scale += offset;
					EditorGUILayout.LabelField(new GUIContent() { text = label, tooltip = tooltip }, GUILayout.Width(100));
					EditorGUILayout.LabelField(" ", GUILayout.Width(3));
					EditorGUILayout.MinMaxSlider(ref offset, ref scale, 0, 1);
					scale -= offset;
				}
			}

			void GetTerrain()
			{
				if (materialTerrain.ContainsKey(targetMat) && materialTerrain[targetMat] != null)
				{
					terrain = materialTerrain[targetMat];
					isOnTerrain = true;
					tLayers = terrain.terrainData.terrainLayers;
				}
				else
                {
					terrain = null;
					isOnTerrain = false;
					tLayers = null;
				}
			}

			void SelectTerrainLayer(int layerNumber, string label)
			{ 				
				string tagName ="TerrainLayerGUID_" + layerNumber.ToString();
				TerrainLayer terainLayer = InTerra_Data.TerrainLayerFromGUID(targetMat, tagName); 

				EditorGUI.BeginChangeCheck();

				using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
				{
					EditorGUILayout.LabelField(LabelAndTooltip(label, "The Terrain Layer the Material will be blended with"), styleLeft, GUILayout.MaxWidth(100));
					Rect rt = GUILayoutUtility.GetLastRect();
					if (terainLayer && AssetPreview.GetAssetPreview(terainLayer.diffuseTexture))
					{
						GUI.DrawTexture(new Rect(rt.x + 103, rt.y, 21, 21), AssetPreview.GetAssetPreview(terainLayer.diffuseTexture), ScaleMode.ScaleToFit, true);
					}

					EditorGUILayout.GetControlRect(GUILayout.Width(20));					
					terainLayer = (TerrainLayer)EditorGUILayout.ObjectField(terainLayer, typeof(TerrainLayer), false, GUILayout.MinWidth(100), GUILayout.Height(22));

					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra TerrainLayer");
						targetMat.SetOverrideTag(tagName, AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(terainLayer)));

						if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
					}
				}
			}
		}
		//=====================================================================================

		MaterialProperty FindProperty(string name)
		{
			return FindProperty(name, properties);
		}

		void ObjectsList(List<Renderer> rend, Color color)
		{
		
			var style = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleLeft };
			if (color != Color.black)
			{
				style.normal.textColor = color;
			}

			for (int i = 0; i< rend.Count; i++)
			{
				GUILayout.BeginHorizontal();
				GUILayout.Label(rend[i].name, style, GUILayout.MinWidth(60));
				GUILayout.Label(rend[i].bounds.center.x.ToString() + ", " + rend[i].bounds.center.y.ToString() + ", " + rend[i].bounds.center.z.ToString(), style, GUILayout.MinWidth(40));

				if (GUILayout.Button("  -->  ", EditorStyles.miniButton, GUILayout.Width(50)))
				{
					Selection.activeGameObject = rend[i].gameObject;
					SceneView.lastActiveSceneView.Frame(rend[i].bounds, false);
				}
				GUILayout.EndHorizontal();
			}		
		}

		void CreateObjectsLists(Material targetMat, Terrain terain)
		{
			Terrain[] terrains = Terrain.activeTerrains;

			if (terrains.Length > 1)
			{
				List<string> terrainTestDuplicite = new List<string>();				
				foreach (var terrain in terrains)
				{
					if (!terrainTestDuplicite.Contains(terrain.name))
					{
						terrainTestDuplicite.Add(terrain.name);
					}
					else
					{
						terrainsDuplicitName = true;
					}

					char[] invalidFileChars = Path.GetInvalidFileNameChars();
					foreach (char invChar in invalidFileChars)
					{
						terrainsInvalidChars = terrainsInvalidChars || terrain.name.Contains(invChar);
					}
				}
			}

			#if UNITY_2023_1_OR_NEWER
				MeshRenderer[] renderers = Object.FindObjectsByType<MeshRenderer>(FindObjectsSortMode.None);
			#else
				MeshRenderer[] renderers = Object.FindObjectsOfType<MeshRenderer>();
			#endif

			okTerrain.Clear();
			noTerrain.Clear();
			wrongTerrain.Clear();
			rendTerrain.Clear();

			foreach (Renderer rend in renderers)
			{
				if (rend != null && rend.transform.position != null)
				{
					foreach (Material mat in rend.sharedMaterials)
					{
						if (mat != null && mat.shader != null && mat.shader.name != null && InTerra_Data.CheckObjectShader(mat))
						{
							Vector2 pos = new Vector2(rend.bounds.center.x, rend.bounds.center.z);
							foreach (var terrain in terrains)
							{
								if (InTerra_Data.CheckPosition(terrain, pos))
								{
									rendTerrain.Add(rend, terrain);
								}
							}

							matContainsInstance = matContainsInstance || mat.name.Contains(" (Instance)");

							if ((mat == targetMat))
							{
								noTerrain.Add(rend); //it is easier to check if the renderer is on Terrain, so all renderes will be add to this list and if it is on terrain, it will be removed 
								wrongTerrain.Add(rend);


								if (InTerra_Data.CheckPosition(terain, pos))
								{
									okTerrain.Add(rend);
									wrongTerrain.Remove(rend);
								}

								foreach (Terrain ter in terrains)
								{
									if (InTerra_Data.CheckPosition(ter, pos))
									{
										noTerrain.Remove(rend);
									}
								}
							}
						}
					}
				}
			}

			foreach (Renderer nt in noTerrain)
			{
				wrongTerrain.Remove(nt);
			}
		}	
	}
}
