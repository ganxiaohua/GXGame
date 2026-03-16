using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace InTerra
{
    public static class InTerra_MaterialManager
    {
		static bool disableUpdates = InTerra_Setting.DisableAllAutoUpdates;
		static bool updateDict = InTerra_Setting.DictionaryUpdate;

		//========================================================================================
		//-----------------------|   MULTIPLE TERRAINS MATERIALS  WARNINGS   |-------------------
		//========================================================================================
		public static void MaterialNameWarnings(Terrain terrain, Material targetMat, string baseName, bool matContainsInstance, bool terrainsDuplicitName, bool terrainsterrainsInvalidChars, Dictionary<Renderer, Terrain> rendTerrain, List<Material> relatedMat)
		{
			GUIStyle richTextLabelStyle = new GUIStyle(GUI.skin.label) { wordWrap = true, alignment = TextAnchor.MiddleLeft, richText = true };

			//-----------------------|  CONTAINS INSTANCES  |-------------------
			if (matContainsInstance)
			{
				using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
				{
					EditorGUILayout.LabelField(EditorGUIUtility.IconContent("console.warnicon"), GUILayout.Height(40), GUILayout.Width(30));
					EditorGUILayout.LabelField("There are some Instanced Materials with InTerra shader, if you want to use <b>Multiple Terrains Material</b> options, no Instanced Materials with  <b><i>Object Into Terrain Integration</i></b> shader are allowed.", richTextLabelStyle);
				}
				GUI.enabled = false;
			}

			//-----------------------|  DUPLICIT NAMES FOR TERRAINS IN SCENE  |-------------------
			if (terrainsDuplicitName)
			{
				EditorGUILayout.HelpBox("There are some Terrains with identical names. Unique name for each Terrain is required, otherwise the following options cannot work properly.", MessageType.Warning);
				GUI.enabled = false;
			}

			//-----------------------|  INVALID CHARACTERS IN TERRAINS NAMES  |-------------------
			if (terrainsterrainsInvalidChars)
			{
				EditorGUILayout.HelpBox("There are invalid characters in some of the Terrains names. Only characters that are allowed for file names are allowed for Terrains names, otherwise the following options cannot work properly.", MessageType.Warning);
				GUI.enabled = false;
			}

			//-----------------------|  MATERIAL NAMES/TAGS INCONSISTENCY  |-------------------
			string terrainTag = targetMat.GetTag("TerrainName", false);

			bool wrongName = targetMat.name != TagsName(targetMat);
			bool wrongTerrainName = terrain && (terrainTag.Length > 0) && terrainTag != terrain.name;

			if (wrongTerrainName || wrongName)
			{
				using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
				{
					EditorGUILayout.LabelField(EditorGUIUtility.IconContent("console.warnicon"), GUILayout.Height(40), GUILayout.Width(30));

					using (new GUILayout.VerticalScope())
					{
						if (wrongName)
                        {
							EditorGUILayout.LabelField("The Material name and Tags name do not match!", richTextLabelStyle);

							if (wrongTerrainName)
							{
								EditorGUILayout.LabelField("The Terrain tag <b>\" " + targetMat.GetTag("TerrainName", false) + "\"</b> of this Material does not match the Terrain name \" " + terrain.name + "\" where the Material is placed. You can try to <b>Reassigne Related Materials</b> or change the Terrain tag.", richTextLabelStyle);
							}
						}
						else if (wrongTerrainName)
						{
							EditorGUILayout.LabelField("The Terrain Name <b>\" " + targetMat.GetTag("TerrainName", false) + "\"</b> in this Material name and Tag does not match the Terrain name <b>\" " + terrain.name + "\"</b> where the Material is placed.", richTextLabelStyle);
						}
						EditorGUILayout.Space();
					}
				}				
			}
		}
		//========================================================================================


		public static void CreateRelatedMaterialsList(string baseName, List<Material> relatedMat)
		{
			Material[] materials = Resources.FindObjectsOfTypeAll(typeof(Material)) as Material[];
			relatedMat.Clear();

			foreach (Material mat in materials)
			{
				if (mat.shader != null && mat.shader.name != null && baseName.Length != 0 && baseName == mat.GetTag("BaseName", false) && !mat.name.Contains("(Instance)"))
				{
					if (!relatedMat.Contains(mat))
					{
						relatedMat.Add(mat);
					}				
				}
			}
		}

		public static void CopyPropertiesToRelated(Material targetMat, List<Material> relatedMat)
		{
			List<Material> copyToMats = new List<Material>();

			foreach (Material mat in relatedMat)
			{
				if (mat != targetMat && InTerra_Data.CheckObjectShader(mat))
				{
					copyToMats.Add(mat);

				}
			}

			Undo.RecordObjects(copyToMats.ToArray(), "InTerra Copy Materials Properties");
			foreach (Material mat in copyToMats)
			{
				string baseTag = mat.GetTag("BaseName", false);
				string terrainTag = mat.GetTag("TerrainName", false);

				mat.CopyPropertiesFromMaterial(targetMat);

				mat.SetOverrideTag("BaseName", baseTag);
				mat.SetOverrideTag("TerrainName", terrainTag);
			}

			if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);		
		}

		static string TagsName(Material mat)
		{
			string tagName = mat.GetTag("BaseName", false) + "_" + mat.GetTag("TerrainName", false);

			return tagName;
		}

		public static void ReassignMaterials(string baseName, List<Material> relatedMat, Dictionary<Renderer, Terrain> rendTerrain)
		{
			foreach (Renderer rend in rendTerrain.Keys)
			{								
				if (relatedMat.Contains(rend.sharedMaterial) || (baseName == rend.sharedMaterial.name))
				{
					Terrain[] terrains = Terrain.activeTerrains;
					foreach (Material mat in relatedMat)
					{
						Terrain  matTerrain = rendTerrain[rend];
						Vector2 pos = new Vector2(rend.bounds.center.x, rend.bounds.center.z);
						if (InTerra_Data.CheckPosition(rendTerrain[rend], pos))
						{
							CheckAndReassign(baseName, mat, rend, matTerrain);
						}
						else
						{
							foreach (Terrain checkTerrain in terrains)
							{
								if (InTerra_Data.CheckPosition(checkTerrain, pos))
								{
									CheckAndReassign(baseName, mat, rend, checkTerrain);
								}
							}
						}										
					}
				}
			}
			if (!disableUpdates) InTerra_Data.UpdateTerrainData(updateDict);				
		}

		static void CheckAndReassign(string baseName, Material mat, Renderer rend, Terrain terrain)
		{
			Dictionary<Renderer, Material> objMat = new Dictionary<Renderer, Material>();

			if (baseName + "_" + terrain.name == TagsName(mat) && baseName + "_" + terrain.name == mat.name)
			{
				objMat.Add(rend, mat);
			}

			Undo.RecordObjects(objMat.Keys.ToArray(), "InTerra Materials Reassigned");
			foreach (Renderer obj in objMat.Keys)
			{
				obj.sharedMaterial = objMat[rend];
			}
		}

		public static void MaterialTags( ref bool changingTerrainTag, Material mat, ref Terrain terrain, MaterialEditor materialEditor, GUIStyle styleMini)
		{
			using (new GUILayout.HorizontalScope())
			{
				EditorGUILayout.LabelField("Bese Name :", styleMini, GUILayout.Width(80));
				EditorGUILayout.LabelField(mat.GetTag("BaseName", false), EditorStyles.helpBox, GUILayout.MinWidth(100));
			}
			using (new GUILayout.HorizontalScope())
			{

				EditorGUILayout.LabelField("Terrain Name :", styleMini, GUILayout.Width(80));

				if (changingTerrainTag)
				{
					if (terrain == null)
					{
						Terrain[] terrains = Terrain.activeTerrains;
						foreach (Terrain ter in terrains)
						{
							if (ter.name == mat.GetTag("TerrainName", false)) terrain = ter;
						}
					}

					EditorGUI.BeginChangeCheck();
					terrain = (Terrain)EditorGUILayout.ObjectField(terrain, typeof(Terrain), true, GUILayout.MinWidth(100), GUILayout.Height(22));

					if (EditorGUI.EndChangeCheck())
					{
						materialEditor.RegisterPropertyChangeUndo("InTerra Terrain Tag Changed");
						mat.SetOverrideTag("TerrainName", terrain.name);
						changingTerrainTag = false;
					}

					if (GUILayout.Button("Cancel"))
					{
						changingTerrainTag = false;
					}
				}
				else
				{
					EditorGUILayout.LabelField(mat.GetTag("TerrainName", false), EditorStyles.helpBox, GUILayout.MinWidth(100));
					if (GUILayout.Button("Change"))
					{
						changingTerrainTag = true;
					}
				}
			}
		}
	}
}
