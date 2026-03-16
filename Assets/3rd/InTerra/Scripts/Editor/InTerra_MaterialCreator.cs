using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;

namespace InTerra
{
    public class InTerra_MaterialCreator : EditorWindow
    {
        static Material SelectedMaterial;
        static string baseName = "";
        static string path = "";
        static string newBase = "";

        static List<string> createdBaseNames;

        static Dictionary<string, Terrain> newMaterial;
        static Dictionary<Material, Terrain> notTageddMat;
        static Dictionary<Renderer, Terrain> materialRendTerrain;
        static Dictionary<Renderer, Terrain> allRendTerrain;
        static List<Material> relatedMaterials;
        static List<Renderer> baseNameRenderers;
        static Terrain[] terrains;

        bool changingName;
        bool wrongName;
        bool setedTags;
        static bool baseNameAlreadyExist;

        Vector2 ScrollPos;
        Vector2 ScrollPos2;
        Vector2 ScrollPos3;

        //===============================================================================================
        //----------------------|   MULTIPLE TERRAINS MATERIALS CREATOR    |-----------------------------
        //===============================================================================================
        public static void OpenWindow(Material Mat, Dictionary<Renderer, Terrain> allRendTer, List<Material> relatedMat, string bName)
        {
            InTerra_MaterialCreator window = GetWindow<InTerra_MaterialCreator>(true, "Multiple Terrains Materials Creator", true);
            baseNameAlreadyExist = relatedMat != null && Mat && relatedMat.Count > 0 && Mat.GetTag("BaseName", false).Length == 0;
            Vector2 size = baseNameAlreadyExist ? new Vector2(809, 475) : new Vector2(809, 390);

            window.minSize = size;
            window.maxSize = size;
            InTerra_Data.CenterOnMainWin(window);

            SelectedMaterial = Mat;
            baseName = bName;

            path = DirectoryPath(SelectedMaterial);

            if (SelectedMaterial != null)
            {
                terrains = Terrain.activeTerrains;
                newMaterial = new Dictionary<string, Terrain>();

                createdBaseNames = new List<string>();
                notTageddMat = new Dictionary<Material, Terrain>();
                allRendTerrain = allRendTer;
                materialRendTerrain = new Dictionary<Renderer, Terrain>();

                relatedMaterials = relatedMat;
                baseNameRenderers = new List<Renderer>();

                RendededsOfMaterial(allRendTer);

                Material[] materials = Resources.FindObjectsOfTypeAll(typeof(Material)) as Material[];
                foreach(Material mat in materials)
                {

                    string baseName = mat.GetTag("BaseName", false);
                    if (baseName.Length > 0)
                    {
                        createdBaseNames.Add(baseName);
                    }
                }
                CreateNewMatList();
            }

            newBase = baseName;
        }


        void OnGUI()
        {
            if (!SelectedMaterial || (newMaterial == null || newMaterial.Count == 0) && (relatedMaterials == null || relatedMaterials.Count == 0) && (notTageddMat == null || notTageddMat.Count == 0))
            {
                Close();
            }

            var normalStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft };
            var wrongStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft};
            wrongStyle.normal.textColor = Color.red;
            wrongStyle.hover.textColor = Color.red;

            var warningStyle = new GUIStyle(EditorStyles.label) { alignment = TextAnchor.MiddleLeft };
            warningStyle.normal.textColor = new Color(1.0f, 0.5f, 0.0f);
            warningStyle.hover.textColor = new Color(1.0f, 0.5f, 0.0f);

            string labelInfo = "";

            if (setedTags)
            {
                baseNameAlreadyExist = false;
                setedTags = false;
            }


            //----------------------|   EXISTING BASE NAME   |-----------------------------
            if (baseNameAlreadyExist)
            {
                GUIStyle breakingLabelStyle = new GUIStyle(GUI.skin.label) { wordWrap = true, alignment = TextAnchor.MiddleLeft, richText = true };
                using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                {
                    EditorGUILayout.LabelField(EditorGUIUtility.IconContent("console.warnicon"), GUILayout.Height(30), GUILayout.Width(30));
                    EditorGUILayout.LabelField("The Material name is the same as the Base name  <b>" + baseName + "</b> of already created set of Materials.", breakingLabelStyle);
                }
                if (GUILayout.Button("Replace Material \"" + baseName + "\"  With Already Created Material(s) and Create New If Needed"))
                {
                    CreateMaterials();
                }
                
                EditorGUILayout.Space();
                GUI.enabled = false;
            }


            //----------------------|   BASE NAME   |-----------------------------
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Materials Base Name:", EditorStyles.miniLabel, GUILayout.Width(115));

                if (!changingName)
                {
                    GUILayout.Label(baseName, new GUIStyle(EditorStyles.helpBox) { wordWrap = false,  fontSize = 12}, GUILayout.Width(425));
                    
                    string tooltip = "";
                    if (wrongName)
                    {
                        GUI.enabled = false;
                        tooltip = "You cannot change Base Name if some materials has incorrect names.";

                    }
                    if (GUILayout.Button(new GUIContent() { text = "Change Base Name", tooltip = tooltip }, EditorStyles.miniButton))
                    {
                        changingName = true;
                    }
                }
                else
                {
                    RenameField(ref changingName, true);
                }
            }

            //----------------------|   PATH   |-----------------------------
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Saving Path:", EditorStyles.miniLabel, GUILayout.Width(115));
                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.TextField(path, new GUIStyle(EditorStyles.helpBox) { wordWrap = false }, GUILayout.Width(425));
                    GUI.enabled = (relatedMaterials == null || relatedMaterials.Count == 0) && (notTageddMat == null || notTageddMat.Count == 0); 

                    if (GUILayout.Button(new GUIContent() { text = "Change", tooltip = "You cannot change the folder if there are already created Materials with this Base name." }, EditorStyles.miniButton))
                    {
                        string absPath = EditorUtility.SaveFolderPanel("Save Materials to folder", path, "");
                        if (absPath.Contains(Application.dataPath))
                        {
                            path = absPath.Substring(absPath.IndexOf("Assets"));
                        }
                        else if (!string.IsNullOrEmpty(absPath))
                        {
                            path = absPath;
                        }
                    }
                   GUI.enabled = !baseNameAlreadyExist;
                }
            }
            Repaint(); 
            EditorGUILayout.Space();

           
            using (new GUILayout.HorizontalScope())
            {
                using (new GUILayout.VerticalScope())
                {
                    //----------------------|   NOT TAGED MATERIALS   |-----------------------------
                    if (notTageddMat != null && notTageddMat.Count > 0)
                    {

                       Vector2 size = new Vector2(809, 390);

                        minSize = size;
                        maxSize = size;

                        using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(400)))
                        {

                            EditorGUILayout.HelpBox("Following Materials are created, but do not have tags for relation recognizing.", MessageType.Warning);
                            ScrollPos2 = EditorGUILayout.BeginScrollView(ScrollPos2);
                            foreach (var mat in notTageddMat.Keys)
                            {

                                labelInfo = "\n\nThis Material does not have needed Tags.";
                                GUIStyle labelStyle = warningStyle;

                                if (!InTerra_Data.CheckObjectShader(mat))
                                {
                                    labelInfo += "\n\nThis Material does not have InTerra shader.";
                                    labelStyle = wrongStyle;
                                }

                                if (GUILayout.Button(new GUIContent() { text = mat.name, tooltip = AssetDatabase.GetAssetPath(mat) + labelInfo }, labelStyle))
                                {
                                    EditorGUIUtility.PingObject(mat);
                                }

                            }
                            EditorGUILayout.EndScrollView();
                        }
                        using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(400)))
                        {
                            SetTags();
                        }
                    }


                    //----------------------|   NEW MATERIALS FOR CREATION   |-----------------------------
                    if (newMaterial != null && !(newMaterial.Count == 0 && notTageddMat.Count > 0))
                    {                       
                        using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(400)))
                        {

                            EditorGUILayout.LabelField("Materials that can be created:", EditorStyles.helpBox);
                            {
                                ScrollPos = EditorGUILayout.BeginScrollView(ScrollPos);

                                foreach (var matName in newMaterial.Keys)
                                {
                                    GUILayout.Label(matName, normalStyle);
                                }


                                EditorGUILayout.EndScrollView();
                            }
                            GUI.enabled = newMaterial.Count > 0;
                            
                        }

                        using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
                        {
                            if (GUILayout.Button(" \n Create and Assign Materials \n ", new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold }))
                            {
                                CreateMaterials();
                                if (!InTerra_Setting.DisableAllAutoUpdates) InTerra_Data.UpdateTerrainData(true);
                            }
                        }
                        GUI.enabled = !baseNameAlreadyExist;
                    }                    
                }           
                
                using (new GUILayout.VerticalScope())
                {
                    //----------------------|   CREATED MATERIALS  |-----------------------------
                    using (new GUILayout.VerticalScope(EditorStyles.helpBox, GUILayout.Width(400)))
                    {
                        EditorGUILayout.LabelField("Created Materials:", EditorStyles.helpBox);
                       
                        if (notTageddMat != null && relatedMaterials != null)
                        {
                            ScrollPos3 = EditorGUILayout.BeginScrollView(ScrollPos3);
                            foreach (var mat in relatedMaterials)
                            {
                                if (!mat) Close();                               
                                GUIStyle labelStyle = normalStyle;                            
                                
                                if (mat && mat.name != TagsName(mat))
                                {
                                    labelInfo = "\n\nThis Material Name and Tags does not match.";
                                    labelStyle = warningStyle;

                                    wrongName = true;
                                }

                                if(!InTerra_Data.CheckObjectShader(mat)) 
                                {
                                    labelInfo += "\n\nThis Material does not have InTerra shader.";
                                    labelStyle = wrongStyle;
                                }

                                if (GUILayout.Button(new GUIContent() { text = mat.name, tooltip = AssetDatabase.GetAssetPath(mat) + labelInfo }, labelStyle))
                                {
                                    EditorGUIUtility.PingObject(mat);
                                }                     
                            }
                            EditorGUILayout.EndScrollView();                           
                        }
                    }

                    GUI.enabled = (relatedMaterials != null && relatedMaterials.Count > 0) && !baseNameAlreadyExist;
                    
                    if (wrongName)
                    {
                        EditorGUILayout.HelpBox("Some materials has incorect names or tags, please fix the issue!", MessageType.Warning);
                    }
                    DeleteMaterials(SelectedMaterial);
                }
            }
        }

        void DeleteMaterials(Material SelectedMaterial)
        {
            using (new GUILayout.HorizontalScope(EditorStyles.helpBox))
            {
                if (GUILayout.Button(" \n Delete Created Materials and Replace them with One...\n ", new GUIStyle(GUI.skin.button) { fontStyle = FontStyle.Bold }))
                {
                    InTerra_DeleteMaterials.OpenWindow(baseName, relatedMaterials, baseNameRenderers);               
                }
            }
        }

        void RenameField(ref bool namingActive, bool renameBaseName)
        {
            using (new GUILayout.HorizontalScope())
            {
                newBase = EditorGUILayout.TextField(newBase, GUILayout.Width(425));

                GUI.SetNextControlName("RenameOk");
                if (GUILayout.Button("OK"))
                {
                    namingActive = false;
                    if (newBase != baseName)
                    {
                        if (NewBaseNameCheck(newBase))
                        {
                            if (renameBaseName)
                            {
                                if (EditorUtility.DisplayDialog("InTerra Renaming", "Renaming of Base name(s) cannot be undone. Do you want to continue?", "Yes", "Cancel"))
                                {
                                    Rename(newBase);
                                    CreateNewMatList();
                                }
                                else
                                {
                                    newBase = baseName;
                                }
                            }
                            else
                            {                               
                                relatedMaterials.Clear();
                                baseName = newBase;
                                CreateNewMatList();

                                baseNameAlreadyExist = false;
                                namingActive = false;
                                Vector2 size = new Vector2(809, 390);

                                minSize = size;
                                maxSize = size;
                            }
                        }
                        else
                        {
                            newBase = baseName;
                        }
                        GUI.FocusControl("CancelButton");
                    }
                }

                GUI.SetNextControlName("CancelButton");
                if (GUILayout.Button("Cancel"))
                {
                    namingActive = false;
                    newBase = baseName;
                    Repaint();
                    GUI.FocusControl("CancelButton");
                }
            }
        }

        void Rename(string newBaseName)
        {
            bool fileExist = false;

            List<string> rename = new List<string>();
            string fullPath;
            if (relatedMaterials != null)
            {
                Material[] materials = Resources.FindObjectsOfTypeAll(typeof(Material)) as Material[];
                foreach (var mat in relatedMaterials)
                {
                    string directoryPath = DirectoryPath(mat);
                    fullPath = directoryPath + "/" + newBaseName + "_" + mat.GetTag("TerrainName", false) + ".mat";

                    if (File.Exists(fullPath))
                    {
                        fileExist = true;
                    }
                }

                if (newMaterial != null)
                {
                    foreach (string matName in newMaterial.Keys)
                    {
                        fullPath = path + "/" + newBaseName + "_" + newMaterial[matName].name + ".mat";
                        if (File.Exists(fullPath) && ((Material)AssetDatabase.LoadAssetAtPath(fullPath, typeof(Material))).GetTag("BaseName", false).Length > 0)
                        {
                            fileExist = true;
                        }
                    }
                }

                if (fileExist)
                {
                    EditorUtility.DisplayDialog("InTerra", "Base name could not be changed because there already are file(s) with such name(s).", "Ok");
                    newBase = baseName;
                }
                else
                {
                    foreach (var mat in relatedMaterials)
                    {
                        if (File.Exists(DirectoryPath(mat) + "/" + newBaseName + "_" + mat.GetTag("TerrainName", false) + ".mat"))
                        {
                            EditorUtility.DisplayDialog("InTerra", "Renaming of the file \"" + AssetDatabase.GetAssetPath(mat) + "\" failed.", "Ok"); ;
                        }
                        else
                        {
                            AssetDatabase.RenameAsset(AssetDatabase.GetAssetPath(mat), newBaseName + "_" + mat.GetTag("TerrainName", false));
                            AssetDatabase.Refresh();

                            if (mat.name == Path.GetFileNameWithoutExtension(AssetDatabase.GetAssetPath(mat)))
                            {
                                mat.SetOverrideTag("BaseName", newBaseName);
                            }
                        }
                    }
                    baseName = newBaseName;
                }               
            }
            CreateNewMatList();
        }

        bool NewBaseNameCheck(string newBaseName)
        {
            createdBaseNames.Clear();
            bool baseNameOk = false;
            bool dotInTerrainName = false;

            foreach (Terrain terrain in terrains)
            {
                if (terrain.name.Contains("."))
                {
                    dotInTerrainName = true;
                }
            }
                
            if (createdBaseNames.Contains(newBaseName))
            {
                EditorUtility.DisplayDialog("InTerra", "This Base name is already used for different set of Materials, please choose a different one.", "Ok");
            }
            else if (InvalidFileChars(newBaseName))
            {
                EditorUtility.DisplayDialog("InTerra", "The Base name contains invalid characters.", "Ok");
            }
            else if (dotInTerrainName)
            {
                EditorUtility.DisplayDialog("InTerra", "The change of base name is not allowed if some Terrain names contains a dot character because of Unity bug.", "Ok");
            }
            else
            {
                baseNameOk = true;
            }
            return baseNameOk;
        }


        static bool InvalidFileChars(string flieName)
        {
            bool invalidCharacter = string.IsNullOrEmpty(flieName) || flieName.Substring(0, 1) == " ";

            char[] invalidFileChars = Path.GetInvalidFileNameChars();
            foreach (char invChar in invalidFileChars)
            {
                invalidCharacter = invalidCharacter || flieName.Contains(invChar);
            }
            return invalidCharacter;
        }


        static void CreateNewMatList()
        {
            newMaterial.Clear();
            notTageddMat.Clear();

            foreach (Terrain terrain in terrains)
            {
                string newMatName = baseName + "_" + terrain.name;
                string pathAndFile = path + "/" + baseName + "_" + terrain.name + ".mat";

                if (!File.Exists(pathAndFile) && materialRendTerrain.ContainsValue(terrain))
                {
                    newMaterial.Add(newMatName, terrain);
                }
                else
                {
                    Material mat = (Material)AssetDatabase.LoadAssetAtPath(pathAndFile, typeof(Material));
                    if (mat && mat.GetTag("BaseName", false).Length == 0 && !notTageddMat.ContainsKey(mat))
                    {
                        notTageddMat.Add(mat, terrain); 
                    }
                }
            }
        }

        void CreateMaterials()
        {
            if (!Directory.Exists(path) || !(path.Length > 3))
            {
                EditorUtility.DisplayDialog("InTerra", "Materials cannot be created because the saving path is not valid.", "Ok");
            }
            else
            {
                Material mat;
                Terrain[] terrains = Terrain.activeTerrains;

                Dictionary<Renderer, Material> objMat = new Dictionary<Renderer, Material>();
                foreach (Terrain terrain in terrains)
                {
                    if (materialRendTerrain.ContainsValue(terrain))
                    {
                        string pathAndFile = path + "/" + baseName + "_" + terrain.name + ".mat";

                        if (!File.Exists(pathAndFile) && newMaterial.ContainsKey(baseName + "_" + terrain.name))
                        {
                            mat = new Material(SelectedMaterial.shader);
                            mat.CopyPropertiesFromMaterial(SelectedMaterial);

                            mat.SetOverrideTag("BaseName", baseName);
                            mat.SetOverrideTag("TerrainName", terrain.name);

                            AssetDatabase.CreateAsset(mat, pathAndFile);
                            AssetDatabase.SaveAssets();
                        }
                        else
                        {
                            mat = (Material)AssetDatabase.LoadAssetAtPath(pathAndFile, typeof(Material));
                        }
                        foreach (Renderer rend in materialRendTerrain.Keys)
                        {
                            if (materialRendTerrain[rend] == terrain)
                            {
                                objMat.Add(rend, mat);
                            }
                        }
                        if (!InTerra_Setting.DisableAllAutoUpdates) InTerra_Data.UpdateTerrainData(true);
                    }
                }
             
                Undo.RecordObjects(objMat.Keys.ToArray(), "InTerra Created/Assigned Materials");
                foreach(Renderer rend in objMat.Keys)
                {
                    rend.sharedMaterial = objMat[rend];
                }
                Close();
            }
        }

        static void RendededsOfMaterial(Dictionary<Renderer, Terrain> allRendTer)
        {
            materialRendTerrain.Clear();
            foreach (Renderer rend in allRendTer.Keys)
            {
                if (rend.sharedMaterial.GetTag("BaseName", false) == baseName) baseNameRenderers.Add(rend);

                foreach (Material mat in rend.sharedMaterials)
                {

                    if ((mat != null && mat.shader.name != null && (mat == SelectedMaterial)) || rend.sharedMaterial.GetTag("BaseName", false) == baseName)
                    {
                        Vector2 position = new Vector2(rend.bounds.center.x, rend.bounds.center.z);
                        foreach (Terrain terrain in terrains)
                        {
                            if (InTerra_Data.CheckPosition(terrain, position))
                            {
                                materialRendTerrain.Add(rend, terrain);

                            }
                        }
                    }
                }
            }
        }
  

        void SetTags()
        {
            Terrain[] terrains = Terrain.activeTerrains;

            if (GUILayout.Button("\nSet Tags And Reassign Materials If Needed\n"))
            {
                Undo.RegisterCompleteObjectUndo(notTageddMat.Keys.ToArray(), "InTerra Set Tags for Material(s)");
                foreach (Material mat in notTageddMat.Keys)
                {
                    mat.SetOverrideTag("BaseName", baseName);
                    mat.SetOverrideTag("TerrainName", notTageddMat[mat].name);
                }

                InTerra_MaterialManager.CreateRelatedMaterialsList(baseName, relatedMaterials);
                InTerra_MaterialManager.ReassignMaterials(baseName, relatedMaterials, allRendTerrain);
                CreateNewMatList();
                notTageddMat.Clear();
                setedTags = true;

                Material newSelected = Selection.activeGameObject.GetComponent<Renderer>().sharedMaterial;

                if (newSelected != null)
                {
                    SelectedMaterial = newSelected;
                }
                else
                {
                    Close();
                }
            }
        }

        static string DirectoryPath(Material mat)
        {
            string assetPath = AssetDatabase.GetAssetPath(mat);
            string directoryPath = "";

            if (assetPath.Length > 0)
            {
                directoryPath = Path.GetDirectoryName(assetPath);
                directoryPath = directoryPath.Replace("\\", "/");  
            }

            return directoryPath;
        }

        static string TagsName(Material mat)
        {
            string tagName = mat.GetTag("BaseName", false) + "_" + mat.GetTag("TerrainName", false);
            return tagName;
        }
    }
}

