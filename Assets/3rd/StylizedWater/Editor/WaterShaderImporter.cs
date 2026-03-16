//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

#if UNITY_2020_2_OR_NEWER
using UnityEditor.AssetImporters;
#else
using UnityEditor.Experimental.AssetImporters;
#endif

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using Debug = UnityEngine.Debug;
using Object = UnityEngine.Object;

namespace StylizedWater2
{
    [ScriptedImporterAttribute(AssetInfo.SHADER_GENERATOR_VERSION, TARGET_FILE_EXTENSION, 2)]
    public class WaterShaderImporter : ScriptedImporter
    {
        private const string TARGET_FILE_EXTENSION = "watershader";
        private const string ICON_NAME = "water-shader-icon";
        
        [Tooltip("Rather than storing the template in this file, it can be sourced from an external text file" +
                 "\nUse this if you intent to duplicate this asset, and need only minor modifications to its import settings")]
        [SerializeField] public LazyLoadReference<TextAsset> template;
            
        [Space]
        
        public WaterShaderSettings settings;

        /// <summary>
        /// File paths of any file this shader depends on. This list will be populated with any "#include" paths present in the template
        /// Registering these as dependencies is required to trigger the shader to recompile when these files are changed
        /// </summary>
        //[NonSerialized] //Want to keep these serialized. Will differ per-project, which also causes the file to appear as changed for every project when updating the asset (this triggers a re-import)
        public List<string> dependencies = new List<string>();

        private bool HasExternalTemplate()
        {////
            #if UNITY_2020_1_OR_NEWER
            return template.isSet;
            #else
            return template.asset;
            #endif
        }
        public string GetTemplatePath()
        {
            return HasExternalTemplate() ? AssetDatabase.GetAssetPath(template.asset) : assetPath;
        }

        private void OnValidate()
        {
            if(settings.shaderName == string.Empty) settings.shaderName = $"{Application.productName} ({DateTime.Now.Ticks})";
        }

        public override void OnImportAsset(AssetImportContext context)
        {
            Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(context.assetPath);
            //if (shader != null) ShaderUtil.ClearShaderMessages(shader);
            
            string templatePath = GetTemplatePath();

            if (templatePath == string.Empty)
            {
                Debug.LogError("Failed to import water shader, template file path is null. It possibly hasn't been imported first?", shader);
                return;
            }
            
            #if SWS_DEV
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            
            string[] lines = File.ReadAllLines(templatePath);

            if (lines.Length == 0)
            {
                Debug.LogError("Failed to generated water shader. Template or file content is empty (or wasn't yet imported)...");
                return;
            }

            dependencies.Clear();
            
            string templateContents = ShaderConfigurator.TemplateParser.CreateShaderCode(context.assetPath, ref lines, this, false);
            
            Shader shaderAsset = ShaderUtil.CreateShaderAsset(templateContents, true);
            ShaderUtil.RegisterShader(shaderAsset);
            
            Texture2D thumbnail = Resources.Load<Texture2D>(ICON_NAME);
            if(!thumbnail) thumbnail = EditorGUIUtility.IconContent("ShaderImporter Icon").image as Texture2D;
            
            context.AddObjectToAsset("MainAsset", shaderAsset, thumbnail);
            context.SetMainObject(shaderAsset);
            
            //Do not attempt to create a tessellation variant for the underwater post-effect shaders
            if (settings.type == WaterShaderSettings.ShaderType.WaterSurface)
            {
                //Re-read the original template again
                lines = File.ReadAllLines(templatePath);
                templateContents = ShaderConfigurator.TemplateParser.CreateShaderCode(context.assetPath, ref lines, this, true);

                Shader tessellation = ShaderUtil.CreateShaderAsset(templateContents, true);
                //ShaderUtil.RegisterShader(tessellation);
                
                context.AddObjectToAsset("Tessellation", (Object)tessellation, thumbnail);
            }
            
            //Set up dependency, so that changes to the template triggers shaders to regenerate
            if (HasExternalTemplate() && AssetDatabase.TryGetGUIDAndLocalFileIdentifier(template, out var guid, out long _))
            {
                //Note: this strictly only works when adding the file path!
                //context.DependsOnArtifact(guid);
                
                dependencies.Insert(0, AssetDatabase.GUIDToAssetPath(guid));
            }

            //Dependencies are populated during the template parsing phase.
            foreach (string dependency in dependencies)
            {
                context.DependsOnSourceAsset(dependency);
            }
            
            #if SWS_DEV
            sw.Stop();
            //Debug.Log($"Imported \"{Path.GetFileNameWithoutExtension(assetPath)}\" water shader in {sw.Elapsed.Milliseconds}ms. With {dependencies.Count} dependencies.", shader);
            #endif
        }

        public void ClearCache(bool recompile = false)
        {
            var objs = AssetDatabase.LoadAllAssetsAtPath(assetPath);
            
            foreach (var obj in objs)
            {
                if (obj is Shader)
                {
                    ShaderUtil.ClearShaderMessages((Shader)obj);
                    ShaderUtil.ClearCachedData((Shader)obj);
                    
                    if(recompile) AssetDatabase.ImportAsset(assetPath);
                    
                    #if SWS_DEV
                    Debug.Log($"Cleared cache for {obj.name}");
                    #endif
                }
            }
        }
        public void RegisterDependency(string dependencyAssetPath)
        {
            if (dependencyAssetPath.StartsWith("Packages/") == false)
            {
                string guid = AssetDatabase.AssetPathToGUID(dependencyAssetPath);

                if (guid == string.Empty)
                {
                    //Also throws an error for things like '#include_library "SurfaceModifiers/SurfaceModifiers.hlsl"', which are wrapped in an #ifdef. That's a false positive
                    //Debug.LogException(new Exception($"Tried to import \"{this.assetPath}\" with an missing dependency, supposedly at path: {dependencyAssetPath}."));
                    return;
                }
            }

            //Tessellation variant pass may run, causing the same dependencies to be registered twice, hence check first
            if(dependencies.Contains(dependencyAssetPath) == false) dependencies.Add(dependencyAssetPath);
        }
        
        //Handles correct behaviour when double-clicking a .watershader asset. Should open in the IDE
        [UnityEditor.Callbacks.OnOpenAsset]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Object target = EditorUtility.InstanceIDToObject(instanceID);

            if (target is Shader)
            {
                var path = AssetDatabase.GetAssetPath(instanceID);
                
                if (Path.GetExtension(path) != "." + TARGET_FILE_EXTENSION) return false;

                string externalScriptEditor = ScriptEditorUtility.GetExternalScriptEditor();
                if (externalScriptEditor != "internal")
                {
                    InternalEditorUtility.OpenFileAtLineExternal(path, 0);
                }
                else
                {
                    Application.OpenURL("file://" + path);
                }
                
                return true;
            }
            
            return false;
        }

        [Serializable]
        public class Directive
        {
            public enum Type
            {
                [InspectorName("(no prefix)")]
                custom,
                [InspectorName("#include")]
                include,
                [InspectorName("#pragma")]
                pragma,
                [InspectorName("#define")]
                define
            }
            public Type type;
            public string value;

            public Directive(Type _type, string _value)
            {
                this.type = _type;
                this.value = _value;
            }
        }
        
        class WaterShaderAssetPostProcessor : AssetPostprocessor
        {
            static void OnPostprocessAllAssets(string[] importedAssets, string[] deletedAssets, string[] movedAssets, string[] movedFromAssetPaths)
            {
                RegisterShaders(importedAssets);
            }

            //Register imported water shaders, so they work with Shader.Find() and show up in the shader selection menu
            private static void RegisterShaders(string[] paths)
            {
                foreach (var path in paths)
                {
                    if (!path.EndsWith(WaterShaderImporter.TARGET_FILE_EXTENSION, StringComparison.InvariantCultureIgnoreCase))
                        continue;

                    var mainObj = AssetDatabase.LoadMainAssetAtPath(path);
                    if (mainObj is Shader)
                    {
                        if (mainObj.name == string.Empty) return;
                        
                        //ShaderUtil.RegisterShader((Shader)mainObj);
                        
                        #if SWS_DEV
                        //Debug.Log($"Registered water shader \"{mainObj.name}\" on import", mainObj);
                        #endif

                        return;
                    }
                }
            }
        }

        public static string[] FindAllAssets()
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Application.dataPath);
            
            FileInfo[] fileInfos = directoryInfo.GetFiles("*." + TARGET_FILE_EXTENSION, SearchOption.AllDirectories);
            
            #if SWS_DEV
            Debug.Log($"{fileInfos.Length} .{TARGET_FILE_EXTENSION} assets found");
            #endif

            string[] filePaths = new string[fileInfos.Length];

            for (int i = 0; i < filePaths.Length; i++)
            {
                filePaths[i] = fileInfos[i].FullName.Replace(@"\", "/").Replace(Application.dataPath, "Assets");
            }

            return filePaths;
        }
        
        #if SWS_DEV
        [MenuItem("SWS/Reimport water shaders")]
        #endif
        public static void ReimportAll()
        {
            string[] filePaths = FindAllAssets();
            foreach (var filePath in filePaths)
            {
                #if SWS_DEV
                Debug.Log($"Reimporting: {filePath}");
                #endif
                AssetDatabase.ImportAsset(filePath);
            }
        }

        [Serializable]
        public class WaterShaderSettings
        {
            [Tooltip("How it will appear in the selection menu")]
            public string shaderName;
            [Tooltip("Hide the shader in the selection menu. Yet still make it findable with Shader.Find()")]
            public bool hidden;
            public enum ShaderType
            {
                WaterSurface,
                PostProcessing
            }
            public ShaderType type;
            
            [Tooltip("Before compiling the shader, check whichever asset is present in the project and activate its integration")]
            public bool autoIntegration = true;
            public ShaderConfigurator.Fog.Assets fogIntegration = ShaderConfigurator.Fog.Assets.UnityFog;

            public bool quadNormalMapSamples = false;
            
            [Tooltip("These are defined in a HLSLINCLUDE block and apply to all passes" +
                     "\nMay be used to implement custom code")]
            public List<Directive> customIncludeDirectives = new List<Directive>();
        }
    }
    
    [CustomEditor(typeof(WaterShaderImporter))]
    [CanEditMultipleObjects]
    public class ShaderImporterEditor: ScriptedImporterEditor
    {
        private WaterShaderImporter importer;

        private SerializedProperty template;
        
        private SerializedProperty settings;

        private SerializedProperty shaderName;
        private SerializedProperty hidden;
        private SerializedProperty type;

        private SerializedProperty autoIntegration;
        private SerializedProperty fogIntegration;
        
        private SerializedProperty customIncludeDirectives;

        private bool underwaterRenderingInstalled;
        private bool surfaceModifiersInstalled;
        private ShaderConfigurator.Fog.Integration firstIntegration;
        private bool curvedWorldInstalled;
        
        private bool showDependencies;
        
        public override void OnEnable()
        {
            base.OnEnable();

            underwaterRenderingInstalled = StylizedWaterEditor.UnderwaterRenderingInstalled();
            surfaceModifiersInstalled = StylizedWaterEditor.SurfaceModifiersInstalled();
            firstIntegration = ShaderConfigurator.Fog.GetFirstInstalled();
            curvedWorldInstalled = StylizedWaterEditor.CurvedWorldInstalled(out var _);
            
            importer = (WaterShaderImporter)target;

            template = serializedObject.FindProperty("template");
            
            settings = serializedObject.FindProperty("settings");
            //settings.isExpanded = true;
            
            shaderName = settings.FindPropertyRelative("shaderName");
            hidden = settings.FindPropertyRelative("hidden");
            type = settings.FindPropertyRelative("type");
            
            autoIntegration = settings.FindPropertyRelative("autoIntegration");
            fogIntegration = settings.FindPropertyRelative("fogIntegration");
            
            customIncludeDirectives = settings.FindPropertyRelative("customIncludeDirectives");
        }

        public override bool HasPreviewGUI()
        {
            //Hide the useless sphere preview :)
            return false;
        }

        public override void OnInspectorGUI()
        {
            //base.OnInspectorGUI();
            Color defaultColor = GUI.contentColor;

            UI.DrawHeader();
            
            Shader shader = AssetDatabase.LoadAssetAtPath<Shader>(importer.assetPath);
            if (shader == null)
            {
                UI.DrawNotification("Shader failed to compile, try to manually recompile it now.", MessageType.Error);
            }
            
            if (GUILayout.Button(new GUIContent("  Recompile", EditorGUIUtility.IconContent("RotateTool").image), GUILayout.MinHeight(30f)))
            {
                importer.SaveAndReimport();
            }
            
            GUILayout.Space(-2f);
            using (new EditorGUILayout.HorizontalScope())
            {
                using (new EditorGUI.DisabledGroupScope(shader == null))
                {
                    if (GUILayout.Button(new GUIContent("  Show Generated Code", EditorGUIUtility.IconContent("align_horizontally_left_active").image), EditorStyles.miniButtonLeft, GUILayout.Height(28f)))
                    {
                        importer = (WaterShaderImporter)target;
                        
                        string path = $"{Application.dataPath.Replace("Assets", string.Empty)}Temp/{importer.settings.shaderName}(Generated Code).shader";

                        string code = ShaderConfigurator.TemplateParser.CreateShaderCode(importer.GetTemplatePath(), importer);
                        File.WriteAllText(path, code);

                        OpenGeneratedCode(path);
                    }
                    if (GUILayout.Button(new GUIContent("Clear cache", "Unity's shader compiler will cache the compiled shader, and internally use that." +
                                                                       "\n\nThis may result in seemingly false-positive shader errors. Such as in the case of importing the shader, before the URP shader libraries are." +
                                                                       "\n\nClearing the cache gives the compiler a kick, and makes the shader properly represent the current state of the project/dependencies."), EditorStyles.miniButtonRight, GUILayout.Height(28f)))
                    {
                        importer.ClearCache();
                    }
                }
            }
                        
            EditorGUILayout.Space();
            
            serializedObject.Update();
            EditorGUI.BeginChangeCheck();

            EditorGUILayout.PropertyField(template);
            
            if(template.objectReferenceValue == null) EditorGUILayout.HelpBox("• Template is assumed to be in the contents of the file itself", MessageType.None);
            //EditorGUILayout.LabelField(importer.GetTemplatePath(), EditorStyles.miniLabel);
            
            EditorGUILayout.Space();

            EditorGUILayout.LabelField("Settings", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(shaderName);
            EditorGUI.indentLevel++;
            EditorGUILayout.PropertyField(hidden);
            EditorGUI.indentLevel--;
            
            EditorGUILayout.PropertyField(type);

            if (type.intValue == (int)WaterShaderImporter.WaterShaderSettings.ShaderType.WaterSurface)
            {
                EditorGUILayout.Space();

                EditorGUILayout.LabelField("Integrations", EditorStyles.boldLabel);

                EditorGUILayout.PropertyField(autoIntegration, new GUIContent("Automatic detection", autoIntegration.tooltip));
                if (autoIntegration.boolValue)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.LabelField("Fog post-processing", GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
                        EditorGUI.indentLevel--;

                        using (new EditorGUILayout.HorizontalScope(EditorStyles.textField))
                        {
                            GUI.contentColor = Color.green;
                            EditorGUILayout.LabelField(firstIntegration.name);

                            GUI.contentColor = defaultColor;
                        }
                    }
                    
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        EditorGUI.indentLevel++;
                        EditorGUILayout.LabelField("Curved World 2020", GUILayout.MaxWidth(EditorGUIUtility.labelWidth));
                        EditorGUI.indentLevel--;

                        using (new EditorGUILayout.HorizontalScope(EditorStyles.textField))
                        {
                            if (curvedWorldInstalled)
                            {
                                GUI.contentColor = Color.green;
                                EditorGUILayout.LabelField("Installed");
                            }
                            else
                            {
                                GUI.contentColor = new Color(1f, 0.65f, 0f);
                                EditorGUILayout.LabelField("(Not installed)");
                            }
                    
                            GUI.contentColor = defaultColor;
                        }
                    }
                }
                else
                {
                    EditorGUILayout.PropertyField(fogIntegration);
                }
                if(curvedWorldInstalled) EditorGUILayout.HelpBox("Curved World integration must be activated through Window->Amazing Assets->Curved Word (Activator tab)", MessageType.Info);
            }
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField("Extensions", EditorStyles.boldLabel);

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Underwater Rendering", GUILayout.MaxWidth(EditorGUIUtility.labelWidth));

                using (new EditorGUILayout.HorizontalScope(EditorStyles.textField))
                {
                    if (underwaterRenderingInstalled)
                    {
                        GUI.contentColor = Color.green;
                        EditorGUILayout.LabelField("Installed");
                    }
                    else
                    {
                        GUI.contentColor = new Color(1f, 0.65f, 0f);
                        EditorGUILayout.LabelField("(Not installed)");
                    }
                    
                    GUI.contentColor = defaultColor;
                }
            }
            
            #if SWS_DEV
            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Surface Modifiers", GUILayout.MaxWidth(EditorGUIUtility.labelWidth));

                using (new EditorGUILayout.HorizontalScope(EditorStyles.textField))
                {
                    if (surfaceModifiersInstalled)
                    {
                        GUI.contentColor = Color.green;
                        EditorGUILayout.LabelField("Installed");
                    }
                    else
                    {
                        GUI.contentColor = new Color(1f, 0.65f, 0f);
                        EditorGUILayout.LabelField("(Not installed)");
                    }
                    
                    GUI.contentColor = defaultColor;
                }
            }
            #endif
            
            EditorGUILayout.Space();
            
            EditorGUILayout.PropertyField(customIncludeDirectives);

            if (EditorGUI.EndChangeCheck())
            {
                //Force the parameter to a matching value.
                //This way, if the "auto-integration" option is used, the .meta file will be changed when using the shader in a package, spanning different projects.
                //When switching a different project, the file will be seen as changed and will be re-imported, in turn applying the project-specific integration.
                if (autoIntegration.boolValue)
                {
                    fogIntegration.intValue = (int)firstIntegration.asset;
                }
                
                serializedObject.ApplyModifiedProperties();
            }
      
            this.ApplyRevertGUI();
            
            showDependencies = EditorGUILayout.BeginFoldoutHeaderGroup(showDependencies, $"Dependencies ({importer.dependencies.Count})");

            if (showDependencies)
            {
                this.Repaint();
                
                using (new EditorGUILayout.VerticalScope(EditorStyles.textArea))
                {
                    foreach (string dependency in importer.dependencies)
                    {
                        var rect = EditorGUILayout.BeginHorizontal(EditorStyles.miniLabel);

                        if (rect.Contains(Event.current.mousePosition))
                        {
                            EditorGUIUtility.AddCursorRect(new Rect(Event.current.mousePosition.x, Event.current.mousePosition.y, 27, 27), MouseCursor.Link);
                            EditorGUI.DrawRect(rect, Color.gray * (EditorGUIUtility.isProSkin ? 0.66f : 0.20f));
                        }
                        
                        if (GUILayout.Button(dependency == string.Empty ? 
                            new GUIContent(" (Missing)", EditorGUIUtility.IconContent("console.warnicon.sml").image) : 
                            new GUIContent(" " + dependency, EditorGUIUtility.IconContent("TextAsset Icon").image), 
                            EditorStyles.miniLabel, GUILayout.Height(20f)))
                        {
                            if (dependency != string.Empty)
                            {
                                TextAsset file = AssetDatabase.LoadAssetAtPath<TextAsset>(dependency);
                                
                                EditorGUIUtility.PingObject(file);
                            }
                        }
                        EditorGUILayout.EndHorizontal();
                    }
                }
                
                EditorGUILayout.HelpBox("Should any of these files be modified/moved/deleted, this shader will also re-import", MessageType.Info);
            }
            EditorGUILayout.EndFoldoutHeaderGroup();

            UI.DrawFooter();
            
            if (shader)
            {
                UI.DrawNotification(ShaderUtil.GetShaderMessageCount(shader) > 0, "Errors may be false-positives due to caching", "Clear cache", ()=> importer.ClearCache(true), MessageType.Warning);
            }
        }

        private void OpenGeneratedCode(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Debug.LogError(string.Format("Path {0} doesn't exists", filePath));
                return;
            }

            string externalScriptEditor = ScriptEditorUtility.GetExternalScriptEditor();
            if (externalScriptEditor != "internal")
            {
                InternalEditorUtility.OpenFileAtLineExternal(filePath, 0);
            }
            else
            {
                Application.OpenURL("file://" + filePath);
            }
        }
    }
}