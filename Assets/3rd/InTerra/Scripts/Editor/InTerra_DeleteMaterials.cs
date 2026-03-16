using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace InTerra
{
    public class InTerra_DeleteMaterials : EditorWindow
    {
        static string baseName;

        static Vector2 ScrollPosMats;

        static Material selectedMaterial = null;
        
        static List<Material> deleteMaterials;
        static List<Renderer> materialsRenderes;
        
        public static void OpenWindow(string materialsBaseName, List<Material> relatedMaterials, List<Renderer> renderes)
        {
            InTerra_DeleteMaterials window = GetWindow<InTerra_DeleteMaterials>(true, "Delete Materials and Replace them...", true);
            Vector2 size = new Vector2(500, 220);
            window.minSize = size;
            window.maxSize = size;

            InTerra_Data.CenterOnMainWin(window);

            deleteMaterials = relatedMaterials;
            baseName = materialsBaseName;
            materialsRenderes = renderes;
        }

        void OnGUI()
        {
            GUIStyle warningLabelStyle;
            warningLabelStyle = new GUIStyle(GUI.skin.label);
            warningLabelStyle.wordWrap = true;
            warningLabelStyle.alignment = TextAnchor.MiddleLeft;
            warningLabelStyle.richText = true;

            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope())
                {
                  
                    EditorGUILayout.LabelField("Following Materials will be deleted:");
                    using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                    {

                        ScrollPosMats = EditorGUILayout.BeginScrollView(ScrollPosMats, GUILayout.Height(100));
                        if (deleteMaterials != null)
                        {
                            foreach (var mat in deleteMaterials)
                            {
                                if (mat != selectedMaterial)
                                {
                                    GUILayout.Label(new GUIContent() { text = mat.name, tooltip = AssetDatabase.GetAssetPath(mat) }, GUILayout.MinWidth(30));
                                }
                            }
                        }
                        else
                        {
                            Close();
                        }
                        EditorGUILayout.EndScrollView();

                    }
                }
            }
            EditorGUILayout.Space();

            using (new GUILayout.VerticalScope(EditorStyles.helpBox))
            {

                EditorGUILayout.LabelField("Select Material for Replacing:");

                selectedMaterial = (Material)EditorGUILayout.ObjectField(selectedMaterial, typeof(Material), false, GUILayout.MinWidth(100), GUILayout.Height(22));

                string replaceMatBaseName = selectedMaterial == null ? "" : selectedMaterial.GetTag("BaseName", false); 

                if (replaceMatBaseName.Length > 0 && replaceMatBaseName != baseName)
                {
                    EditorUtility.DisplayDialog("Selected Material", "To avoid inconsistency you cannot choose Material that belongs to another set of Materials with different Base name, please choose a different one.", "Ok");
                    selectedMaterial = null;
                }
            }
            EditorGUILayout.Space();
            using (new GUILayout.HorizontalScope())
            {

                if(selectedMaterial == null)
                {
                    GUI.enabled = false;

                }
                if (GUILayout.Button(" OK "))
                {

                    if (EditorUtility.DisplayDialog("Delete Materials", "All Materials with the base name " + baseName + " will be deleted and to all Renderers using these Materials will be asigned the Material " + selectedMaterial.name + ". This step cannot be undo.", "Ok", "Cancel"))
                    {
                        foreach (var mat in deleteMaterials)
                        {
                            if (mat != selectedMaterial)
                            {
                                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(mat));
                                AssetDatabase.Refresh();
                            }
                        }
                        deleteMaterials.Clear();
                        InTerra_MaterialManager.CreateRelatedMaterialsList(baseName, deleteMaterials);

                        foreach (Renderer rend in materialsRenderes)
                        {
                            rend.sharedMaterial = selectedMaterial;

                        }
                    }

                    selectedMaterial = null;
                    Close();
                   

                }

                GUI.enabled = true;

                if (GUILayout.Button(" Cancel "))
                {
                    selectedMaterial = null;
                    Close();
                }
            }
            
        }

    }
}
