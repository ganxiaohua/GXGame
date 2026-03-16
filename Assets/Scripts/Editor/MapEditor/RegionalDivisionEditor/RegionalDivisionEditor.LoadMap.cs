using System.Collections.Generic;
using System.IO;
using GameFrame.Runtime;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay.Editor.MapEditor
{
    public partial class RegionalDivisionEditor
    {
        [LabelText("地图id")] [HorizontalGroup("地图加载")]
        public int MapId;

        [ShowInInspector] [LabelText("当前地图快大小")] [ReadOnly] [ShowIf("@mapEditor!=null && mapEditor.AreaData!=null")]
        public Vector2Int CurChunkSize;

        [ShowInInspector] [LabelText("需要设置地图块大小")]
        public Vector2Int ChunkSize = new Vector2Int(50, 50);

        [HorizontalGroup("地图加载")]
        [Button("加载地图")]
        public void LoadMap()
        {
            var activeScene = SceneManager.GetActiveScene();
            if (activeScene.path != EditorConst.editorScenePath)
            {
                EditorCommon.GoScene(EditorConst.editorScenePath);
            }

            var rootObjects = SceneManager.GetActiveScene().GetRootGameObjects();
            foreach (var obj in rootObjects)
            {
                Object.DestroyImmediate(obj);
            }

            string path = string.Format(ConstPath.MapPath, MapId);
            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog(
                        "没有这个地图",
                        "你必须先建立一个地基，存放在Res/Prefabs/Map下",
                        "确定"
                );
                return;
            }

            var map = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            mapEditor.Map = (GameObject) PrefabUtility.InstantiatePrefab(map);
            LoadMapData();
        }

        private void LoadMapData()
        {
            string path = string.Format(ConstPath.MapDataPath, MapId);
            if (!File.Exists(path))
            {
                mapEditor.AreaData = ScriptableObject.CreateInstance<AreaData>();
                AssetDatabase.CreateAsset(mapEditor.AreaData, path);
                mapEditor.AreaData.Id = MapId;
                DivideTheMap();
                CreateChunk();
                mapEditor.AreaData.ChunkCount = mapEditor.AreaData.CellSize.x * mapEditor.AreaData.CellSize.y;
                AssetDatabase.SaveAssets();
            }
            else
            {
                mapEditor.AreaData = AssetDatabase.LoadAssetAtPath<AreaData>(path);
            }

            CurChunkSize = mapEditor.AreaData.ChunkSize;
        }

        [Button("保存设置")]
        [ShowIf("@mapEditor!=null && mapEditor.AreaData!=null")]
        private void MarkAreaDirtyAndSave()
        {
            if (mapEditor.AreaData != null)
            {
                if (mapEditor.AreaData.ChunkSize != ChunkSize)
                {
                    CoordinateRedefinition(ChunkSize);
                }

                EditorUtility.SetDirty(mapEditor.AreaData);
                AssetDatabase.SaveAssets();
            }
        }

        private void DivideTheMap()
        {
            var data = Find();
            var max = data[1];
            var min = data[0];
            var area = mapEditor.AreaData;
            area.ChunkSize = ChunkSize;
            area.StartPoint = new Vector2Int((int) min.x, (int) min.y);
            area.Size = new Vector2Int((int) (max.x - min.x), (int) (max.y - min.y));
            area.CellSize = new Vector2Int(area.Size.x / area.ChunkSize.x + (area.Size.x % area.ChunkSize.x != 0 ? 1 : 0),
                    area.Size.y / area.ChunkSize.y + (area.Size.y % area.ChunkSize.y != 0 ? 1 : 0));
            MarkAreaDirtyAndSave();
        }

        private List<AreaChunkData> CreateChunk()
        {
            List<AreaChunkData> chunks = new List<AreaChunkData>();
            int count = mapEditor.AreaData.CellSize.x * mapEditor.AreaData.CellSize.y;
            for (int index = 0; index < count; index++)
            {
                var path = string.Format(ConstPath.MapChunkPath, MapId, index);
                AreaChunkData areaChunkData;
                areaChunkData = CreateInstance<AreaChunkData>();
                areaChunkData.Id = index;
                chunks.Add(areaChunkData);
                AssetDatabase.CreateAsset(areaChunkData, path);
                areaChunkData.Units = new List<Unit>();
            }

            return chunks;
        }

        private void CoordinateRedefinition(Vector2Int newSize)
        {
            int count = mapEditor.AreaData.CellSize.x * mapEditor.AreaData.CellSize.y;
            List<Unit> units = new List<Unit>();
            for (int index = 0; index < count; index++)
            {
                var path = string.Format(ConstPath.MapChunkPath, MapId, index);
                AreaChunkData areaChunkData = AssetDatabase.LoadAssetAtPath<AreaChunkData>(path);
                if (areaChunkData.Units != null)
                    units.AddRange(areaChunkData.Units);
                AssetDatabase.DeleteAsset(path);
            }

            ChunkSize = newSize;
            DivideTheMap();
            AssetDatabase.Refresh();
            var chunkList = CreateChunk();
            for (int i = 0; i < units.Count; i++)
            {
                var index = mapEditor.GetChunkIndexWithPos(units[i].lPos);
                units[i].Index = chunkList[index].Units.Count;
                chunkList[index].Units.Add(units[i]);
            }

            CurChunkSize = mapEditor.AreaData.ChunkSize;
            LoadMap();
        }
    }
}