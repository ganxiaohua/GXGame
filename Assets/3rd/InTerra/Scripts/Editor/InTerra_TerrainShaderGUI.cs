using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEditor;

namespace InTerra
{
	public class InTerra_TerrainShaderGUI : ShaderGUI
	{
		bool setMinMax = false;
		bool setNormScale = false;
		bool moveLayer = false;
		bool pomSetting = false;
		bool tessSetting = false;
		bool tessDistances = false;
		bool layersScales = false;
		bool colorTintLayers = false;
		bool colorTintTexture = false;
		bool normalTintTexture = false;		
		bool mipMinMax = false;
		bool normDistMinMax = false;

		int layerToFirst = 0;
		string shaderName = " ";

		enum TessellationMode
		{
			[Description("None")] None,
			[Description("Phong")] Phong
		}

		public override void OnGUI(MaterialEditor materialEditor, MaterialProperty[] properties)
		{
			Material targetMat = materialEditor.target as Material;
			bool normInMask = targetMat.IsKeywordEnabled("_TERRAIN_NORMAL_IN_MASK");
			bool disableUpdates = InTerra_Setting.DisableAllAutoUpdates;
			bool updateDict = InTerra_Setting.DictionaryUpdate;

			//----------------------------- FONT STYLES ----------------------------------
			var styleButtonBold = new GUIStyle(GUI.skin.button) {fontStyle = FontStyle.Bold};
			var styleBoldCenter = new GUIStyle(EditorStyles.boldLabel) { alignment = TextAnchor.MiddleCenter };
			var styleBold = new GUIStyle(EditorStyles.boldLabel);
			var styleRight = new GUIStyle(GUI.skin.label) { alignment = TextAnchor.MiddleRight };
			var styleMini = new GUIStyle(EditorStyles.miniLabel) { alignment = TextAnchor.MiddleRight };
			var styleBigBold = new GUIStyle(GUI.skin.label) { fontStyle = FontStyle.Bold, fontSize = 13 };
			//----------------------------------------------------------------------------

			Terrain terrain = null;
			if (Selection.activeGameObject != null)
			{
				terrain = Selection.activeGameObject.GetComponent<Terrain>();
			}

			if (terrain == null)
			{
				EditorGUILayout.HelpBox("No Terrain is selected, some settings may not be available!", MessageType.Warning);
			}

			if (targetMat.shader.name != shaderName)
			{
				if (targetMat.shader.name == InTerra_Data.DiffuseTerrainShaderName)
				{
					SetDiffuseTintKeyword();
				}
				if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);			
				shaderName = targetMat.shader.name;
			}


			if (targetMat.shader.name == InTerra_Data.DiffuseTerrainShaderName)
			{
				using (new GUILayout.VerticalScope(EditorStyles.helpBox))
				{
					HeightmapBlending("Heightmap blending", "Heightmap based texture transition.");					
				}
			}
			else
			{
				//=============================  MASK MAP FEATURES  =============================
				using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
				{
					EditorGUILayout.LabelField("Mask Map Features", styleBoldCenter);

					//-------------- TESSELLATION ----------------
					if (targetMat.shader.name.Contains(InTerra_Data.HDRPTerrainTessellationShaderName))
					{
						using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
						{
							EditorGUILayout.LabelField("Tessellation", styleBoldCenter);
							EditorGUILayout.Space();

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
								materialEditor.RegisterPropertyChangeUndo("InTerra Tessellation Mode");
								SetKeyword("_TESSELLATION_PHONG", tessMode == TessellationMode.Phong);
								if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
							}
							PropertyLine("_TessellationShadowQuality", "Shadows quality", "Setting of shadows accuracy calculation. Higher value means more precise calculation.");
							

							EditorGUILayout.Space();

							EditorGUI.indentLevel = 1;
							tessDistances = EditorGUILayout.Foldout(tessDistances, "Fading Distances", true);
							EditorGUI.indentLevel = 0;
							if (tessDistances && terrain != null)
							{
								InTerra_GUI.TessellationDistaces(targetMat, materialEditor, ref mipMinMax);
							}							

							EditorGUI.indentLevel = 1;
							tessSetting = EditorGUILayout.Foldout(tessSetting, "Layers Setting", true);
							EditorGUI.indentLevel = 0;
							
							if (tessSetting && terrain != null)
							{
								float maxTessHeight = 0;
								for (int i = 0; i < terrain.terrainData.alphamapLayers; i++) 
								{
									TerrainLayer tl = terrain.terrainData.terrainLayers[i];
									if (tl)
									{
										Vector4 remapMax = tl.diffuseRemapMax;
										Vector4 remapMin = tl.diffuseRemapMin;
										float amplitude = remapMax.w;
										float centrer = remapMin.z * 100;
										float height = (amplitude / 2) + centrer;

										maxTessHeight = maxTessHeight < height ? height : maxTessHeight;

										using (new GUILayout.VerticalScope(EditorStyles.helpBox))
										{
											EditorGUI.BeginChangeCheck();

											using (new GUILayout.HorizontalScope())
											{
												EditorGUILayout.LabelField((i + 1).ToString() + ". " + tl.name, styleBold, GUILayout.MinWidth(100));
											}

											using (new GUILayout.HorizontalScope())
											{
												if (tl && AssetPreview.GetAssetPreview(tl.diffuseTexture))
												{
													GUI.Box(EditorGUILayout.GetControlRect(GUILayout.Width(40), GUILayout.Height(40)), AssetPreview.GetAssetPreview(tl.diffuseTexture));
												}

												using (new GUILayout.VerticalScope())
												{
													amplitude = EditorGUILayout.FloatField("Amplitude :", amplitude);
													centrer = EditorGUILayout.FloatField("Height Offset:", centrer);
												}
											}
	
											remapMax.w = Mathf.Clamp(amplitude, 0, 100);
											remapMin.z = Mathf.Clamp(centrer, -50, 50) * 0.01f;
											
											if (EditorGUI.EndChangeCheck())
											{
												Undo.RecordObject(terrain.terrainData.terrainLayers[i], "InTerra Tessellation Values");
												tl.diffuseRemapMin = remapMin;
												tl.diffuseRemapMax = remapMax;
											}
										}
									}
								}
								targetMat.SetFloat("_TessellationMaxDisplacement", maxTessHeight * 0.01f);
								
							}
							EditorGUI.indentLevel = 0;
						}
					}

					//------------------------ HEIGHTMAP BLENDING ------------------------
					HeightmapBlending("Heightmap Blending", "Heightmap based texture transition.");

					//------------------------ PARALLAX OCCLUSION MAPPING ------------------------
					if (!targetMat.shader.name.Contains(InTerra_Data.HDRPTerrainTessellationShaderName))
					{
						bool parallax = targetMat.IsKeywordEnabled("_TERRAIN_PARALLAX");

						EditorGUI.BeginChangeCheck();
						EditorStyles.label.fontStyle = FontStyle.Bold;

						parallax = EditorGUILayout.ToggleLeft(LabelAndTooltip("Parallax Occlusion Mapping", "An illusion of 3D effect created by offsetting the texture depending on heightmap."), parallax);
						EditorStyles.label.fontStyle = FontStyle.Normal;
						

						if (EditorGUI.EndChangeCheck())
						{
							materialEditor.RegisterPropertyChangeUndo("InTerra Parallax Occlusion Mapping");
							SetKeyword("_TERRAIN_PARALLAX", parallax);
							if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
						}

						if (parallax)
						{
							using (new GUILayout.VerticalScope(EditorStyles.helpBox))
							{
								using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
								{
									EditorGUI.BeginChangeCheck();
									float affineSteps = targetMat.GetFloat("_ParallaxAffineStepsTerrain");
									GUILayout.Label(LabelAndTooltip("Affine Steps: ", "The higher number the smoother transition between steps, but also the higher number will increase performance heaviness."));
									affineSteps = EditorGUILayout.IntField((int)affineSteps, GUILayout.MaxWidth(30));									
									affineSteps = Mathf.Clamp(affineSteps, 1, 10);
									
									if (EditorGUI.EndChangeCheck())
									{
										materialEditor.RegisterPropertyChangeUndo("InTerra Parallax Values");
										targetMat.SetFloat("_ParallaxAffineStepsTerrain", affineSteps);
									}
								}
								using (new GUILayout.VerticalScope(EditorStyles.helpBox))
								{
									InTerra_GUI.MipMapsFading(targetMat, "Mip Maps Fading", materialEditor, ref mipMinMax);
								}
								EditorGUILayout.Space();

								EditorGUI.indentLevel = 1;								
								pomSetting = EditorGUILayout.Foldout(pomSetting, "Layers Setting", true);
								EditorGUI.indentLevel = 0;
								if (pomSetting && terrain != null)
								{
									using (new GUILayout.VerticalScope(EditorStyles.helpBox))
									{

										using (new GUILayout.HorizontalScope())
										{
											GUILayout.Label(LabelAndTooltip("Height", "The value of the height illusion."), styleMini, GUILayout.MinWidth(40));
											GUILayout.Label(LabelAndTooltip("Steps", "Each step is creating a new layer for offsetting. The more steps, the more precise the parallax effect will be, but also the higher number will increase performance heaviness."), styleMini, GUILayout.MaxWidth(30));
										}
										for (int i = 0; i < terrain.terrainData.alphamapLayers; i++)
										{
											TerrainLayer tl = terrain.terrainData.terrainLayers[i];
											if (tl)
											{
												Vector4 amplitude = tl.diffuseRemapMax;
												Vector4 steps = tl.diffuseRemapMin;
												EditorGUI.BeginChangeCheck();
												GUILayout.BeginHorizontal();
												amplitude.w = EditorGUILayout.FloatField((i + 1).ToString() + ". " + tl.name + " :", amplitude.w, GUILayout.MinWidth(60));
												amplitude.w = Mathf.Clamp(amplitude.w, 0, 20);
												steps.w = EditorGUILayout.IntField((int)steps.w, GUILayout.MaxWidth(25));
												steps.w = Mathf.Clamp(steps.w, 0, 50);
												GUILayout.EndHorizontal();
												if (EditorGUI.EndChangeCheck())
												{
													Undo.RecordObject(terrain.terrainData.terrainLayers[i], "InTerra Parallax Values");
													tl.diffuseRemapMax = amplitude;
													tl.diffuseRemapMin = steps;

												}
											}
										}
									}
								}
							}
						}
					}

					//------------------------ OCCLUSION, METALLIC, SMOOTHNESS ------------------------
					bool maskMaps = targetMat.IsKeywordEnabled("_TERRAIN_MASK_MAPS") && !normInMask;
					string omLabel = "A.Occlusion, Metallic, Smoothness";
					string omTooltip = "This option applies the Ambient occlusion, Metallic and Smoothness maps from Mask Map.";
					EditorGUI.BeginChangeCheck();

					EditorStyles.label.fontStyle = FontStyle.Bold;	
					maskMaps = normInMask ? false : EditorGUILayout.ToggleLeft(LabelAndTooltip(omLabel, omTooltip), maskMaps);

					EditorStyles.label.fontStyle = FontStyle.Normal;
					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Terrain Mask maps");
						SetKeyword("_TERRAIN_MASK_MAPS", maskMaps);
						if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
					}
				
					//-------------------------------- NORMAL IN MASK -------------------------------- 				
					EditorGUI.BeginChangeCheck();
					EditorStyles.label.fontStyle = FontStyle.Bold;
					normInMask = EditorGUILayout.ToggleLeft(LabelAndTooltip("Normal map in Mask map", "The Normal map will be taken from Mask map Green and Alpha channel, A.Occlusion from Red channel and Heightmap from Blue."), normInMask);
					EditorStyles.label.fontStyle = FontStyle.Normal;

					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Normal To Mask");
						SetKeyword("_TERRAIN_NORMAL_IN_MASK", normInMask);
						if (normInMask) SetKeyword("_TERRAIN_MASK_MAPS", false);
						if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
					}

					if (!normInMask)
					{
						if (GUILayout.Button(LabelAndTooltip("Mask Map Creator", "Open window for creating Mask Map"), styleButtonBold))
						{
							InTerra_MaskCreator.OpenWindow(false);
						}
					}
					else
					{
						using (new GUILayout.VerticalScope(EditorStyles.helpBox))
						{
							if (GUILayout.Button(LabelAndTooltip("Normal-Mask Map Creator", "Open window for creating Mask Map including Normal map."), styleButtonBold))
							{
								InTerra_MaskCreator.OpenWindow(true);
							}

							EditorGUI.indentLevel = 1;
							setNormScale = EditorGUILayout.Foldout(setNormScale, "Normal Scales", true);

							if (setNormScale && terrain != null)
							{
								for (int i = 0; i < terrain.terrainData.alphamapLayers; i++)
								{
									TerrainLayer tl = terrain.terrainData.terrainLayers[i];
									if (tl)
									{										
										float nScale = tl.normalScale;
										EditorGUI.BeginChangeCheck();
										nScale = EditorGUILayout.FloatField((i + 1).ToString() + ". " + tl.name + " :", nScale);
										if (EditorGUI.EndChangeCheck())
										{
											Undo.RecordObject(terrain.terrainData.terrainLayers[i], "InTerra TerrainLayer Normal Scale");
											tl.normalScale = nScale;
										}
									}
								}
							}
						}
						EditorGUI.indentLevel = 0;
					}
					
				}
			}

			//========================= HIDE TILING (DISTANCE BLENDING) ========================
			bool distanceBlending = targetMat.IsKeywordEnabled("_TERRAIN_DISTANCEBLEND");
			Vector4 distance = targetMat.GetVector("_HT_distance");
		
		
			using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
			{
				EditorGUI.BeginChangeCheck();
				EditorStyles.label.fontStyle = FontStyle.Bold;
				distanceBlending = EditorGUILayout.ToggleLeft(LabelAndTooltip("Hide Tiling", "Hides tiling by covering the texture by its scaled up version in the given distance from the camera."), distanceBlending);
				EditorStyles.label.fontStyle = FontStyle.Normal;

				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo("InTerra HideTiling Keyword");
					SetKeyword("_TERRAIN_DISTANCEBLEND", distanceBlending);
					if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
				}

				EditorGUI.BeginChangeCheck();
				if (distanceBlending)
				{
					using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
					{					
						PropertyLine("_HT_distance_scale", "Scale", "This value is multiplying the scale of the Texture of a distant area.");
						EditorGUI.indentLevel = 1;
						layersScales = EditorGUILayout.Foldout(layersScales, "Adjust Layers Scales", true);
						if (layersScales && terrain != null)
						{
							for (int i = 0; i < terrain.terrainData.alphamapLayers; i++)
							{
								TerrainLayer tl = terrain.terrainData.terrainLayers[i];
								if (tl)
								{
									Vector4 scale = tl.diffuseRemapMin;
									EditorGUI.BeginChangeCheck();
									scale.x = EditorGUILayout.Slider((i + 1).ToString() + ". " + tl.name + " :", scale.x, -1, 1);
									if (EditorGUI.EndChangeCheck())
									{
										Undo.RecordObject(terrain.terrainData.terrainLayers[i], "InTerra Terrain Layers Scales");
										tl.diffuseRemapMin = scale;
									}
								}
							}
						}
						EditorGUI.indentLevel = 0;

						PropertyLine("_HT_cover", "Cover strenght", "Strength of covering the Terrain textures in the distant area.");

						using (new GUILayout.HorizontalScope())
						{
							EditorGUILayout.LabelField(LabelAndTooltip("Distance", "The distance where the covering will start. The closer the sliders are, the sharper is the transition."), GUILayout.Width(70));
							EditorGUILayout.LabelField(distance.x.ToString("0.0"), GUILayout.Width(30));
							EditorGUILayout.MinMaxSlider(ref distance.x, ref distance.y, distance.z, distance.w);
							EditorGUILayout.LabelField(distance.y.ToString("0.0"), GUILayout.Width(30));
						}

						EditorGUI.indentLevel = 1;
						setMinMax = EditorGUILayout.Foldout(setMinMax, "Adjust Distance range", true);
						EditorGUI.indentLevel = 0;
						if (setMinMax)
						{
							using (new GUILayout.HorizontalScope()) 
							{
								EditorGUILayout.LabelField("Min:", styleRight, GUILayout.Width(45));
								distance.z = EditorGUILayout.DelayedFloatField(distance.z, GUILayout.MinWidth(50));

								EditorGUILayout.LabelField("Max:", styleRight, GUILayout.Width(45));
								distance.w = EditorGUILayout.DelayedFloatField(distance.w, GUILayout.MinWidth(50));
							}
						}

						distance.x = Mathf.Clamp(distance.x, distance.z, distance.w);
						distance.y = Mathf.Clamp(distance.y, distance.z, distance.w);

					}
				
				}

				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo("InTerra HideTiling Value");
					targetMat.SetVector("_HT_distance", distance);
				}
			}

			//============================= TRIPLANAR ============================= 
			bool triplanar = targetMat.IsKeywordEnabled("_TERRAIN_TRIPLANAR") || targetMat.IsKeywordEnabled("_TERRAIN_TRIPLANAR_ONE");
			bool triplanarOneLayer = targetMat.IsKeywordEnabled("_TERRAIN_TRIPLANAR_ONE");
			bool applyFirstLayer = targetMat.GetFloat("_TriplanarOneToAllSteep") == 1;

			using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
			{
				EditorGUI.BeginChangeCheck();

				EditorStyles.label.fontStyle = FontStyle.Bold;
				triplanar = EditorGUILayout.ToggleLeft(LabelAndTooltip("Triplanar Mapping", "The Texture on steep slopes of Terrain will not be stretched."), triplanar);
				EditorStyles.label.fontStyle = FontStyle.Normal;
				if (triplanar && terrain != null)				
				{
					targetMat.SetVector("_TerrainSize", terrain.terrainData.size); //needed for triplanar UV
				}

				if (triplanar)
				{					
					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						PropertyLine("_TriplanarSharpness", "Sharpness", "Sharpness of the textures transitions between planar projections.");
						triplanarOneLayer = EditorGUILayout.ToggleLeft(LabelAndTooltip("First Layer Only", "Only the first Terrain Layer will be triplanared - this option is for performance reasons."), triplanarOneLayer, GUILayout.MaxWidth(115));

						if (triplanarOneLayer)
						{							
							EditorGUI.indentLevel = 1;
							EditorStyles.label.fontSize = 11;
							applyFirstLayer = EditorGUILayout.ToggleLeft(LabelAndTooltip("Apply first Layer to all steep slopes", "The first Terrain Layer will be automaticly applied to all steep slopes."), applyFirstLayer);
							EditorStyles.label.fontSize = 12;

							if (terrain && terrain.terrainData.alphamapLayers > 1)
							{
								moveLayer = EditorGUILayout.Foldout(moveLayer, "Move Layer To First Position", true);
								EditorGUI.indentLevel = 0;
								if (moveLayer)
								{
									List<string> tl = new List<string>();
									for (int i = 1; i < terrain.terrainData.alphamapLayers; i++)
									{
										tl.Add((i + 1).ToString() + ". " + terrain.terrainData.terrainLayers[i].name.ToString()); 
									}
									if ((layerToFirst + 1) >= terrain.terrainData.alphamapLayers) layerToFirst = 0;
									TerrainLayer terainLayer = terrain.terrainData.terrainLayers[layerToFirst + 1];

									using (new GUILayout.HorizontalScope())
									{									
										if (terainLayer && AssetPreview.GetAssetPreview(terainLayer.diffuseTexture))
										{
											GUI.Box(EditorGUILayout.GetControlRect(GUILayout.Width(50),  GUILayout.Height(50)), AssetPreview.GetAssetPreview(terainLayer.diffuseTexture));		
										}
										else
										{
											EditorGUILayout.GetControlRect(GUILayout.Width(50), GUILayout.Height(50));
										}
										using (new GUILayout.VerticalScope())
										{										
											layerToFirst = EditorGUILayout.Popup("", layerToFirst, tl.ToArray(), GUILayout.MinWidth(170));
											if (GUILayout.Button("Move Layer to First Position", GUILayout.MinWidth(170), GUILayout.Height(27)))
											{											
												MoveLayerToFirstPosition(terrain, layerToFirst + 1);
												if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
											}
										}
										EditorGUILayout.GetControlRect(GUILayout.MinWidth(10));
									}
								}
							}							
						}
					}
				}
				EditorGUI.indentLevel = 0;

				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo("InTerra Triplanar Terrain");
					SetKeyword("_TERRAIN_TRIPLANAR_ONE", triplanar && triplanarOneLayer);
					SetKeyword("_TERRAIN_TRIPLANAR", triplanar && !triplanarOneLayer);
					if (applyFirstLayer && triplanar && triplanarOneLayer) targetMat.SetFloat("_TriplanarOneToAllSteep", 1); else targetMat.SetFloat("_TriplanarOneToAllSteep", 0);
				}

				if (targetMat.GetFloat("_NumLayersCount") > 4)
				{
					EditorGUILayout.HelpBox("Triplanar Features will be applied on Terrain Base Map only if \"First Layer only\" is checked and there are not more than four Layers.", MessageType.Info);
				}
			}

			//========================= TWO LAYERS ONLY ===========================
			bool twoLayers = targetMat.IsKeywordEnabled("_LAYERS_TWO");

			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUI.BeginChangeCheck();

				EditorStyles.label.fontStyle = FontStyle.Bold;
				twoLayers = EditorGUILayout.ToggleLeft(LabelAndTooltip("First Two Layers Only", "The shader will sample only first twoo layers."), twoLayers);
				EditorStyles.label.fontStyle = FontStyle.Normal;
				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo("InTerra Terrain Two Layers");
					SetKeyword("_LAYERS_TWO", twoLayers);
				}
			}

			//========================= COLOR TINT ===========================
			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("Color Tint", styleBigBold);
				EditorGUI.indentLevel = 1;
				colorTintTexture = EditorGUILayout.Foldout(colorTintTexture, "Color Tint Texture", true);
				EditorGUI.indentLevel = 0;
			
				if (colorTintTexture)
				{
					Texture ColorTintTexture = targetMat.GetTexture("_TerrainColorTintTexture");
					float tintStrenght = targetMat.GetFloat("_TerrainColorTintStrenght");

					EditorGUI.BeginChangeCheck();
					using (new GUILayout.HorizontalScope())
					{

						ColorTintTexture = (Texture2D)EditorGUILayout.ObjectField(ColorTintTexture, typeof(Texture2D), false, GUILayout.Height(65), GUILayout.Width(65));
						using (new GUILayout.VerticalScope())
						{
							using (new GUILayout.VerticalScope(EditorStyles.helpBox))
							{
								EditorGUILayout.LabelField("Tint Strenght:", GUILayout.MinWidth(35));
								tintStrenght = EditorGUILayout.Slider(tintStrenght, 0, 1, GUILayout.MinWidth(35));
								EditorGUILayout.LabelField(" ", GUILayout.MinWidth(35));

							}						
						}
					}
					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						materialEditor.TextureScaleOffsetProperty(FindProperty("_TerrainColorTintTexture", properties));
					}	

					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Terrain Color Tint Texture");
						targetMat.SetTexture("_TerrainColorTintTexture", ColorTintTexture);
						targetMat.SetFloat("_TerrainColorTintStrenght", tintStrenght);
						if (targetMat.shader.name == InTerra_Data.DiffuseTerrainShaderName)
						{
							SetDiffuseTintKeyword();
						}
					}
				}

				EditorGUI.indentLevel = 1;
				colorTintLayers = EditorGUILayout.Foldout(colorTintLayers, "Layers Color Tint", true);

				if (colorTintLayers && terrain != null)
				{
					for (int i = 0; i < terrain.terrainData.alphamapLayers; i++)
					{
						TerrainLayer tl = terrain.terrainData.terrainLayers[i];
						if (tl)
						{
							Vector4 color = tl.diffuseRemapMax;
							EditorGUI.BeginChangeCheck();
							color = EditorGUILayout.ColorField(new GUIContent() { text = (i + 1).ToString() + ". " + tl.name} , color, true, false, false);

							if (EditorGUI.EndChangeCheck())
							{
								Undo.RecordObject(terrain.terrainData.terrainLayers[i], "InTerra TerrainLayer Color Tint");
								tl.diffuseRemapMax = color;
							}
						}
					}
				}
				EditorGUI.indentLevel = 0;
			}

			//========================= ADDITIONAL NORMAL ===========================
			using (new GUILayout.VerticalScope(EditorStyles.helpBox))
			{
				EditorGUILayout.LabelField("Additional Normal", styleBigBold);
				EditorGUI.indentLevel = 1;
				normalTintTexture = EditorGUILayout.Foldout(normalTintTexture, "Additional Normal Texture", true);
				EditorGUI.indentLevel = 0;

				if (normalTintTexture)
				{
					Texture normalTintTexture = targetMat.GetTexture("_TerrainNormalTintTexture");
					float tintStrenght = targetMat.GetFloat("_TerrainNormalTintStrenght");
					Vector4 normalDistance = targetMat.GetVector("_TerrainNormalTintDistance");


					EditorGUI.BeginChangeCheck();
					using (new GUILayout.HorizontalScope())
					{

						normalTintTexture = (Texture2D)EditorGUILayout.ObjectField(normalTintTexture, typeof(Texture2D), false, GUILayout.Height(65), GUILayout.Width(65));
						using (new GUILayout.VerticalScope())
						{
							using (new GUILayout.VerticalScope(EditorStyles.helpBox))
							{
								EditorGUILayout.LabelField("Normal Strenght:", GUILayout.MinWidth(35));
								tintStrenght = EditorGUILayout.Slider(tintStrenght, 0, 1, GUILayout.MinWidth(35));
								EditorGUILayout.LabelField(" ", GUILayout.MinWidth(35));

							}
						}
					}
					
					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						materialEditor.TextureScaleOffsetProperty(FindProperty("_TerrainNormalTintTexture", properties));
					}
					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						EditorGUILayout.LabelField("Starting Distance");
						normalDistance = InTerra_GUI.MinMaxValues(normalDistance, true, ref normDistMinMax);
					}

					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Additional Normal");
						targetMat.SetTexture("_TerrainNormalTintTexture", normalTintTexture);
						targetMat.SetFloat("_TerrainNormalTintStrenght", tintStrenght);
						targetMat.SetVector("_TerrainNormalTintDistance", normalDistance);
						if (targetMat.shader.name == InTerra_Data.DiffuseTerrainShaderName)
						{
							SetDiffuseTintKeyword();
						}
					}
				}
			}

			EditorGUILayout.Space();

			using (new GUILayout.VerticalScope(EditorStyles.helpBox)) 
			{
				if (GUILayout.Button(LabelAndTooltip("Update Terrain Data For Objects", "Send updated data from Terrain to Objects integrated to Terrain."), styleButtonBold))
				{
					InTerra_Data.UpdateTerrainData(true);

					//--------- Updating the Materials outside of active Scene ---------
					string[] matGUIDS = AssetDatabase.FindAssets("t:Material", null);

					foreach (string guid in matGUIDS)
					{
						Material mat = (Material)AssetDatabase.LoadAssetAtPath(AssetDatabase.GUIDToAssetPath(guid), typeof(Material));
						if (mat && mat.shader && mat.shader.name != null && !InTerra_Data.GetUpdaterScript().MaterialTerrain.ContainsKey(mat) && InTerra_Data.CheckObjectShader(mat))
						{
							if (mat.IsKeywordEnabled("_LAYERS_ONE"))
							{
								InTerra_Data.TerrainLaeyrDataToMaterial(InTerra_Data.TerrainLayerFromGUID(mat, "TerrainLayerGUID_1"), 0, mat);
							}
							if (mat.IsKeywordEnabled("_LAYERS_TWO"))
							{
								InTerra_Data.TerrainLaeyrDataToMaterial(InTerra_Data.TerrainLayerFromGUID(mat, "TerrainLayerGUID_1"), 0, mat);
								InTerra_Data.TerrainLaeyrDataToMaterial(InTerra_Data.TerrainLayerFromGUID(mat, "TerrainLayerGUID_2"), 1, mat);
							}
						}
					}
				}
			}
			EditorGUILayout.Space();


			if (targetMat.shader.name.Contains(("InTerra/HDRP")))			
			{								
				using (new GUILayout.VerticalScope())
				{
					targetMat.renderQueue = 2225;
					//========================= ENABLE DECALS ===========================
					bool decals = !targetMat.IsKeywordEnabled("_DISABLE_DECALS");
					EditorGUI.BeginChangeCheck();
					decals = EditorGUILayout.Toggle(LabelAndTooltip("Receive Decals", "Enable to allow Materials to receive decals."), decals);
					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Enable Receive Decals");
						SetKeyword("_DISABLE_DECALS", !decals);
					}
				
					//========================= PER PIXEL NORMAL ===========================
					bool perPixelNormal = targetMat.IsKeywordEnabled("_TERRAIN_INSTANCED_PERPIXEL_NORMAL");
					EditorGUI.BeginChangeCheck();
					perPixelNormal = EditorGUILayout.Toggle(LabelAndTooltip("Enable Per-pixel Normal", "Enable per-pixel normal when the terrain uses instanced rendering."), perPixelNormal);
					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Enable Per-pixel Normal");
						SetKeyword("_TERRAIN_INSTANCED_PERPIXEL_NORMAL", perPixelNormal);
					}
				}
			}
			else
			{
				materialEditor.RenderQueueField();
			}

			materialEditor.EnableInstancingField();


			void HeightmapBlending(string name, string tooltip)
			{
				
				bool heightBlending = targetMat.IsKeywordEnabled("_TERRAIN_BLEND_HEIGHT");
				EditorGUI.BeginChangeCheck();
				EditorStyles.label.fontStyle = FontStyle.Bold;
				heightBlending = EditorGUILayout.ToggleLeft(LabelAndTooltip(name, tooltip), heightBlending, GUILayout.MinWidth(120));
				
				EditorStyles.label.fontStyle = FontStyle.Normal;

				if (EditorGUI.EndChangeCheck())
				{
					materialEditor.RegisterPropertyChangeUndo("InTerra HeightBlend");
					SetKeyword("_TERRAIN_BLEND_HEIGHT", heightBlending);
					if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);
				}

				if (heightBlending)
				{
					using (new GUILayout.VerticalScope(EditorStyles.helpBox))
					{
						PropertyLine("_HeightTransition", "Sharpness", "Sharpness of the textures transitions");

						if (targetMat.IsKeywordEnabled("_TERRAIN_DISTANCEBLEND"))
						{
							PropertyLine("_Distance_HeightTransition", "Distant Sharpness", "Sharpness of the textures transitions for distant area setted in Hide Tiling.");
						}

						if (targetMat.shader.name.Contains(InTerra_Data.HDRPTerrainTessellationShaderName))
						{
							PropertyLine("_Tessellation_HeightTransition", "Tessellation Sharpness", "Sharpness of the textures transitions for Tessellation.");
						}

					}
				}
				else
                {
					if (targetMat.shader.name == InTerra_Data.DiffuseTerrainShaderName) EditorGUILayout.LabelField("(Heightmap will be taken from Diffuse Alpha cahnnel)", styleMini);
				}



				if (targetMat.GetFloat("_NumLayersCount") > 4)
				{
					EditorGUILayout.HelpBox("The Heightmap blending will not be applied on Terrain Base Map if there are more than four Layers.", MessageType.Info);
				}
			}

			void PropertyLine(string property, string label, string tooltip = null)
			{
				materialEditor.ShaderProperty(FindProperty(property, properties), new GUIContent() { text = label, tooltip = tooltip });
			}

			void SetKeyword(string name, bool set)
			{
				if (set) targetMat.EnableKeyword(name); else targetMat.DisableKeyword(name);
			}

			void SetDiffuseTintKeyword()
			{
				SetKeyword("_TERRAIN_TINT_TEXTURE",
							(targetMat.GetTexture("_TerrainColorTintTexture") && targetMat.GetFloat("_TerrainColorTintStrenght") > 0) ||
							(targetMat.GetTexture("_TerrainNormalTintTexture") && targetMat.GetFloat("_TerrainNormalTintStrenght") > 0));
			}
		}
		GUIContent LabelAndTooltip(string label, string tooltip)
		{
			return new GUIContent() { text = label, tooltip = tooltip };
		}

		static void MoveLayerToFirstPosition(Terrain terrain, int indexToFirst)
		{
			float[,,] alphaMaps = terrain.terrainData.GetAlphamaps(0, 0, terrain.terrainData.alphamapWidth, terrain.terrainData.alphamapHeight);

			for (int y = 0; y < terrain.terrainData.alphamapHeight; y++)
			{
				for (int x = 0; x < terrain.terrainData.alphamapWidth; x++)
				{
					float a0 = alphaMaps[x, y, 0];
					float a1 = alphaMaps[x, y, indexToFirst];

					alphaMaps[x, y, 0] = a1;
					alphaMaps[x, y, indexToFirst] = a0;

				}
			}
			TerrainLayer[] origLayers = terrain.terrainData.terrainLayers;
			TerrainLayer[] movedLayers = terrain.terrainData.terrainLayers;

			TerrainLayer firstLayer = terrain.terrainData.terrainLayers[0];
			TerrainLayer movingLayer = terrain.terrainData.terrainLayers[indexToFirst];

			movedLayers[0] = movingLayer;
			movedLayers[indexToFirst] = firstLayer;

			terrain.terrainData.SetTerrainLayersRegisterUndo(origLayers, "InTerra Move Terrain Layer");			
			terrain.terrainData.terrainLayers = movedLayers;
			
			Undo.RegisterCompleteObjectUndo(terrain.terrainData.alphamapTextures, "InTerra Move Terrain Layer");
			terrain.terrainData.SetAlphamaps(0, 0, alphaMaps);
		}
	}
}
