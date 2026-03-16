using System.Collections.Generic;
using GameFrame.Runtime;
using GamePlay.Runtime;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    public partial class GridManager : ScriptableSingleton<GridManager>
    {
        private static GameObject selectGameObject => Selection.activeGameObject != null ? Selection.activeGameObject : null;
        private static GridData GridDataSelect => selectGameObject != null ? selectGameObject.GetComponent<GridData>() : null;
        private static GridData GridData;
        public List<GridData> GridDataList;
        private static Material sMaterial;
        private static Mesh sMesh;
        private static int sLastGridProxyHash;
        private static Vector3 offset = new Vector3(0, 0.01f, 0);
        public bool active => GridDataList != null;

        private bool registeredEventHandler;

        private static GUIStyle guiStyle;

        [InitializeOnLoadMethod]
        private static void Initialize()
        {
            instance.RegisterEventHandlers();
        }


        private void OnDisable()
        {
            registeredEventHandler = false;
            SceneView.duringSceneGui -= OnSceneGuiDelegate;
            Selection.selectionChanged -= UpdateCache;
        }

        private void RegisterEventHandlers()
        {
            if (registeredEventHandler)
                return;
            SceneView.duringSceneGui += OnSceneGuiDelegate;
            Selection.selectionChanged += UpdateCache;
            registeredEventHandler = true;
        }


        private void UpdateCache()
        {
            FlushCachedGridProxy();
            ClearAreaMesh();
            SceneView.RepaintAll();
        }


        static void FlushCachedGridProxy()
        {
            if (sMesh == null)
                return;
            DestroyImmediate(sMesh);
            sMesh = null;
            sMaterial = null;
            guiStyle = null;
        }


        private void OnSceneGuiDelegate(SceneView sceneView)
        {
            if (active && sceneView.drawGizmos)
            {
                if (!(GridDataList == null || GridDataList.Count == 0))
                {
                    foreach (var data in GridDataList)
                    {
                        if (data == null)
                            continue;
                        GridData = data;
                        Draw(sceneView);
                    }
                }

                if (GridDataSelect != null)
                {
                    GridData = GridDataSelect;
                    Draw(sceneView);
                }
            }
        }

        private void Draw(SceneView sceneView)
        {
            int gridHash = GenerateHash(GridData);
            if (sLastGridProxyHash != gridHash)
            {
                FlushCachedGridProxy();
                ClearAreaMesh();
                sLastGridProxyHash = gridHash;
            }

            guiStyle ??= new GUIStyle()
            {
                    normal = new GUIStyleState() {textColor = Color.white},
                    fontSize = 12,
            };
            // GetCursorPosWithEditorScene(sceneView);
            // BuildPromptMesh(GridData);
            DrawGrid.DrawGridGizmo(GridData.CroplandData, offset, Color.yellow, ref sMesh, ref sMaterial);
            Handles.Label(GridData.CroplandData.Pos, $"Grid Info\nCellSize:{GridData.CroplandData.CellSize}\n area:{GridData.CroplandData.GirdArea}", guiStyle);
            Vector3 viewportPoint = new Vector3(50, sceneView.camera.pixelHeight, sceneView.camera.nearClipPlane);
            var pos = sceneView.camera.ScreenToWorldPoint(viewportPoint);
            var sb = StringBuilderCache.Get();
            Handles.Label(pos, sb.ToString(), guiStyle);
            StringBuilderCache.Release(sb);
        }

        private static int GenerateHash(GridData layout)
        {
            int hash = 0x7ed55d16;
            hash ^= layout.CroplandData.CellSize.GetHashCode();
            hash ^= layout.CroplandData.GirdArea.GetHashCode() << 23;
            return hash;
        }
    }
}