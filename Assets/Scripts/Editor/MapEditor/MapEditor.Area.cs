using System.Collections.Generic;
using System.IO;
using GameFrame.Runtime;
using UnityEngine;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using UnityEditor;

namespace GamePlay.Editor.MapEditor
{
    public partial class MapEditor
    {
        public AreaData AreaData;

        public GameObject Map;

        private JumpIndexArray<AreaChunkData> chunks;

        private void ShowArea()
        {
            if (AreaData == null)
                return;
            DrawOther.DrawArea(AreaData);
        }

        public JumpIndexArray<AreaChunkData> GetChunks()
        {
            if (AreaData == null)
                return null;
            if (chunks != null)
                return chunks;
            chunks = new JumpIndexArray<AreaChunkData>();
            var count = AreaData.CellSize.x * AreaData.CellSize.y;
            chunks.Init(count);
            return chunks;
        }

        public void SaveChunk()
        {
            if (chunks == null)
                return;
            foreach (var chunk in chunks)
            {
                SaveChunk(chunk);
            }
        }

        public void SaveChunk(AreaChunkData data)
        {
            string str = string.Format(ConstPath.MapAstarAssetPath, AreaData.Id, data.Id);
            data.HaveAStarPath = File.Exists(str);

            EditorUtility.SetDirty(data);
            AssetDatabase.SaveAssets();
        }

        public (AreaChunkData chunk, bool isNew) GetChunkWithPos(Vector3 worldPos)
        {
            var index = GetChunkIndexWithPos(worldPos);
            return GetChunkWithIndex(index);
        }


        private (AreaChunkData chunk, bool isNew) GetChunkWithIndex(int index)
        {
            bool isNew = false;
            var chunks = GetChunks();
            if (chunks == null)
                return (null, false);
            if (chunks[index] != null)
                return (chunks[index], isNew);
            string path = string.Format(ConstPath.MapChunkPath, AreaData.Id, index);
            AreaChunkData areaChunkData;
            if (!File.Exists(path))
            {
                areaChunkData = ScriptableObject.CreateInstance<AreaChunkData>();
                AssetDatabase.CreateAsset(areaChunkData, path);
                areaChunkData.Units = new List<Unit>();
                areaChunkData.Id = index;
                isNew = true;
            }
            else
            {
                areaChunkData = AssetDatabase.LoadAssetAtPath<AreaChunkData>(path);
                isNew = false;
            }

            chunks.Set(index, areaChunkData);
            return (areaChunkData, isNew);
        }

        public int GetChunkIndexWithPos(Vector3 worldPos)
        {
            var norPos = new Vector2(worldPos.x, worldPos.z) - AreaData.StartPoint;
            int chunkX = (int) (norPos.x / AreaData.ChunkSize.x);
            int chunkY = (int) (norPos.y / AreaData.ChunkSize.y);
            int index = chunkY * AreaData.CellSize.x + chunkX;
            return index;
        }
    }
}