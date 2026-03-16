//Stylized Water 2
//Staggart Creations (http://staggart.xyz)
//Copyright protected under Unity Asset Store EULA

using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
#if URP
using UnityEngine.Rendering.Universal;
#if UNITY_2021_2_OR_NEWER
using ForwardRendererData = UnityEngine.Rendering.Universal.UniversalRendererData;
#endif
#endif

namespace StylizedWater2
{
    public class StylizedWaterEditor : Editor
    {
        #if URP
        [MenuItem("GameObject/3D Object/Water/Object", false, 0)]
        public static void CreateWaterObject()
        {
            Material mat = AssetDatabase.LoadAssetAtPath<Material>(AssetDatabase.GUIDToAssetPath("fbb04271505a76f40b984e38071e86f3"));
            Mesh mesh = AssetDatabase.LoadAssetAtPath<Mesh>(AssetDatabase.GUIDToAssetPath("1e01d80fdc2155d4692276500db33fc9"));

            WaterObject obj = WaterObject.New(mat, mesh);
            
            //Position in view
            if (SceneView.lastActiveSceneView)
            {
                obj.transform.position = SceneView.lastActiveSceneView.camera.transform.position + (SceneView.lastActiveSceneView.camera.transform.forward * (Mathf.Max(mesh.bounds.size.x, mesh.bounds.size.z)) * 0.5f);
            }
            
            if (Selection.activeGameObject) obj.transform.parent = Selection.activeGameObject.transform;

            Selection.activeObject = obj;
            
            if(Application.isPlaying == false) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }

        [MenuItem("GameObject/3D Object/Water/Grid", false, 1)]
        [MenuItem("Window/Stylized Water 2/Create water grid", false, 2001)]
        public static void CreateWaterGrid()
        {
            GameObject obj = new GameObject("Water Grid", typeof(WaterGrid));
            Undo.RegisterCreatedObjectUndo(obj, "Created Water Grid");

            obj.layer = LayerMask.NameToLayer("Water");
            
            WaterGrid grid = obj.GetComponent<WaterGrid>();
            grid.Recreate();

            if (Selection.activeGameObject) obj.transform.parent = Selection.activeGameObject.transform;
            
            Selection.activeObject = obj;

            //Position in view
            if (SceneView.lastActiveSceneView)
            {
                Vector3 position = SceneView.lastActiveSceneView.camera.transform.position + (SceneView.lastActiveSceneView.camera.transform.forward * grid.scale * 0.5f);
                position.y = 0f;
                
                grid.transform.position = position;
            }
            
            if(Application.isPlaying == false) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        
        [MenuItem("GameObject/3D Object/Water/Planar Reflections Renderer", false, 2)]
        [MenuItem("Window/Stylized Water 2/Set up planar reflections", false, 2000)]
        public static void CreatePlanarReflectionRenderer()
        {
            GameObject obj = new GameObject("Planar Reflections Renderer", typeof(PlanarReflectionRenderer));
            Undo.RegisterCreatedObjectUndo(obj, "Created PlanarReflectionRenderer");
            PlanarReflectionRenderer r = obj.GetComponent<PlanarReflectionRenderer>();
            r.ApplyToAllWaterInstances();

            Selection.activeObject = obj;
            
            if(Application.isPlaying == false) EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        #endif
        
        [MenuItem("Assets/Create/Water/Mesh")]
        private static void CreateWaterPlaneAsset()
        {
            ProjectWindowUtil.CreateAssetWithContent("New Watermesh.watermesh", "");
        }
        
        [MenuItem("CONTEXT/Transform/Add floating transform")]
        private static void AddFloatingTransform(MenuCommand cmd)
        {
            Transform t = (Transform)cmd.context;

            if (!t.gameObject.GetComponent<FloatingTransform>())
            {
                FloatingTransform component = t.gameObject.AddComponent<FloatingTransform>();
                EditorUtility.SetDirty(t);
            }
        }
        
        public static bool UnderwaterRenderingInstalled()
        {
            //Checking for UnderwaterRenderer.cs meta file
            string path = AssetDatabase.GUIDToAssetPath("6a52edc7a3652d84784e10be859d5807");
            return AssetDatabase.LoadMainAssetAtPath(path);
        }
        
        public static bool SurfaceModifiersInstalled()
        {
            //Checking for the WaterModifiersRenderFeature.cs meta file
            string path = AssetDatabase.GUIDToAssetPath("89138902f514ada418b44ddd62e8de22");
            return AssetDatabase.LoadMainAssetAtPath(path);
        }

        public static void OpenGraphicsSettings()
        {
            SettingsService.OpenProjectSettings("Project/Graphics");
        }
        
        public static void SelectForwardRenderer()
        {
			#if URP
            if (!UniversalRenderPipeline.asset) return;

            System.Reflection.BindingFlags bindings = System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance;
            ScriptableRendererData[] m_rendererDataList = (ScriptableRendererData[])typeof(UniversalRenderPipelineAsset).GetField("m_RendererDataList", bindings).GetValue(UniversalRenderPipeline.asset);

            ForwardRendererData main = m_rendererDataList[0] as ForwardRendererData;
            Selection.activeObject = main;
			#endif
        }

        public static void EnableDepthTexture()
        {
			#if URP
            if (!UniversalRenderPipeline.asset) return;

            UniversalRenderPipeline.asset.supportsCameraDepthTexture = true;
            EditorUtility.SetDirty(UniversalRenderPipeline.asset);
			#endif
        }

        public static void EnableOpaqueTexture()
        {
			#if URP
            if (!UniversalRenderPipeline.asset) return;

            UniversalRenderPipeline.asset.supportsCameraOpaqueTexture = true;
            EditorUtility.SetDirty(UniversalRenderPipeline.asset);
			#endif
        }
        
        /// <summary>
        /// Configures the assigned water material to render as double-sided, which is required for underwater rendering
        /// </summary>
        public static void DisableCullingForMaterial(Material material)
        {
            if (!material) return;
            
            material.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            
            EditorUtility.SetDirty(material);
        }
        
        public static bool CurvedWorldInstalled(out string libraryPath)
        {
            //Checking for "CurvedWorldTransform.cginc"
            libraryPath = AssetDatabase.GUIDToAssetPath("208a98c9ab72b9f4bb8735c6a229e807");
            return libraryPath != string.Empty;
        }

        #if NWH_DWP2
        public static bool DWP2Installed => true;
        #else
        public static bool DWP2Installed => false;
        #endif

        public static T Find<T>() where T : Object
        {
            #if UNITY_2023_1_OR_NEWER
            return (T)Object.FindFirstObjectByType(typeof(T));
            #elif UNITY_2020_1_OR_NEWER
            return (T)Object.FindObjectOfType(typeof(T), false);
            #else
            return (T)Object.FindObjectOfType(typeof(T));
            #endif
        }

        public static void SetupForDWP2()
        {
            #if NWH_DWP2
            if (!EditorUtility.DisplayDialog("Dynamic Water Physics 2 -> Stylized Water 2", 
                "This operation will look for a \"Flat Water Data Provider\" component and replace it with the \"Stylized Water Data Provider\" component", 
                "OK", "Cancel"))
            {
                return;
            }
            
            NWH.DWP2.WaterData.StylizedWaterDataProvider dataProvider = Find<NWH.DWP2.WaterData.StylizedWaterDataProvider>();

            if (dataProvider)
            {
                EditorUtility.DisplayDialog("Dynamic Water Physics 2 -> Stylized Water 2", "A \"Stylized Water Data Provider\" component was already found in the scene", "OK");

                EditorGUIUtility.PingObject(dataProvider.gameObject);
                
                return;
            }
            else
            {
                if (EditorUtility.DisplayDialog("Dynamic Water Physics 2 -> Stylized Water 2", 
                    "Could not find a \"Flat Water Data Provider\" component in the scene.\n\nIt's recommended to first set up DWP2 according to their manual.", 
                    "OK"))
                {
                    return;
                }
            }
            
            NWH.DWP2.WaterData.FlatWaterDataProvider oldProvider = Find<NWH.DWP2.WaterData.FlatWaterDataProvider>();

            NWH.DWP2.DefaultWater.Water waterScript = Find<NWH.DWP2.DefaultWater.Water>();
            if(waterScript) DestroyImmediate(waterScript);
            
            if (oldProvider)
            {
                dataProvider = oldProvider.gameObject.AddComponent<NWH.DWP2.WaterData.StylizedWaterDataProvider>();
                oldProvider.gameObject.AddComponent<StylizedWater2.WaterObject>();
                
                Selection.activeGameObject = oldProvider.gameObject;

                DestroyImmediate(oldProvider);
            }
            #endif
        }
        
    }
}