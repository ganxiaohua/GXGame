// ... existing code ...

using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GamePlay.Runtime;
using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class MapGoCreateEditor : OdinEditorWindow, IMapEditorSceneUpdate
    {
        private static MapGoCreateEditor window;
        private MapEditor mapEditor;
        public bool isClear { get; set; }

        [PropertyOrder(2)]
        [ShowInInspector]
        [LabelText("单位设置")]
        private UnitCreateEditor unitCreateEditor;

        [PropertyOrder(3)]
        [ShowInInspector]
        [LabelText("内空间设置")]
        private HouseCreateEditorEditor houseCreateEditorEditor;

        private bool showGameObjectButton;

        private Dictionary<int, bool> showEditorGosDic;

        private AstarPath astarPath;

        public static MapGoCreateEditor Create()
        {
            window = ScriptableObject.CreateInstance<MapGoCreateEditor>();
            return window;
        }

        public void Init(MapEditor parent)
        {
            showEditorGosDic = new();
            mapEditor = parent;
            isClear = false;
            houseCreateEditorEditor = new HouseCreateEditorEditor();
            houseCreateEditorEditor.Init(parent, this);
            unitCreateEditor = new UnitCreateEditor();
            unitCreateEditor.Init(parent, this);
            showGameObjectButton = false;
            var aStarGo = GameObject.Find("AStar");
            if (aStarGo != null)
            {
                astarPath = aStarGo.GetComponent<AstarPath>();
            }
        }

        public void Clear()
        {
            if (isClear)
                return;
            isClear = true;
            SaveChunk();
            houseCreateEditorEditor?.Clear();
            unitCreateEditor?.Clear();
        }


        public void OnSceneUpdate(SceneView view)
        {
            OnScene();
        }

        private void OnScene()
        {
            if (mapEditor.AreaData == null)
                return;
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            bool raycast = false;
            if (Physics.Raycast(ray, out hit))
            {
                ShowInjectArea(hit.point);
                raycast = true;
            }

            OtherEvent(raycast, e, hit);
            houseCreateEditorEditor.OnScene(raycast, e, hit);
            unitCreateEditor.OnScene(raycast, e, hit);
        }

        private void OtherEvent(bool rayCast, Event e, RaycastHit raycat)
        {
            if (e != null && e.type == EventType.KeyDown)
            {
                if ((e.control || e.command) && e.keyCode == KeyCode.S)
                {
                    SaveChunk();
                }
                else if (e.keyCode == KeyCode.LeftShift)
                {
                    if (rayCast)
                    {
                        Vector3 worldPosition = raycat.point;
                        var chunkData = mapEditor.GetChunkWithPos(worldPosition);
                        ShowOrHideChunkView(chunkData.chunk);
                    }

                    e.Use();
                }
                else if (e.alt)
                {
                    showGameObjectButton = !showGameObjectButton;
                    ShowGameObjectButton();
                    e.Use();
                }
            }
        }


        private void ShowInjectArea(Vector3 mousePos)
        {
            var old = Handles.matrix;
            var norPos = new Vector2(mousePos.x, mousePos.z) - mapEditor.AreaData.StartPoint;
            int chunkX = (int) (norPos.x / mapEditor.AreaData.ChunkSize.x);
            int chunkY = (int) (norPos.y / mapEditor.AreaData.ChunkSize.y);
            int areaIndex = chunkY * (mapEditor.AreaData.CellSize.x) + chunkX;
            Vector3 pos = new Vector3(chunkX * mapEditor.AreaData.ChunkSize.x, 0, chunkY * mapEditor.AreaData.ChunkSize.y) +
                          mapEditor.AreaData.StartPoint.XZCoordinateCVector3();
            var size = mapEditor.AreaData.ChunkSize.XZCoordinateCVector3();
            Handles.matrix = Matrix4x4.TRS(pos + size / 2, Quaternion.identity, Vector3.one);
            Handles.color = Color.blue;
            Handles.DrawWireCube(Vector3.zero, size);
            Handles.matrix = old;
        }
    }
}