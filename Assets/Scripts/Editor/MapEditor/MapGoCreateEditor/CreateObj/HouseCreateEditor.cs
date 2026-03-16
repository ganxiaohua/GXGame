using System;
using System.Collections.Generic;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    [HideReferenceObjectPicker]
    public class HouseCreateEditorEditor :
            CreateEditorBase
    {
        public override void Init(MapEditor mapEditor, MapGoCreateEditor mapGoCreateEditor)
        {
            base.Init(mapEditor, mapGoCreateEditor);
            var hashset = new HashSet<string>();
            hashset.Add(ConstPath.PrefabsHouse);
            unitMatrix.SetDrawData(hashset, false);
        }


        protected override void InjectGo(Vector3 worldPosition)
        {
            var selectPrefabName = unitMatrix.selectedPrefab.name;
            int index = selectPrefabName.LastIndexOf("_", StringComparison.Ordinal);
            var id = selectPrefabName.Substring(index + 1, selectPrefabName.Length - 1 - index);
            var chunkData = mapEditor.GetChunkWithPos(worldPosition);
            if (chunkData.chunk == null)
                return;
            var chunkHouse = new ChunkHouseData();
            chunkHouse.HouseId = int.Parse(id);
            chunkHouse.lPos = worldPosition;
            chunkHouse.lScale = unitMatrix.scale;
            chunkHouse.lRot = Quaternion.Euler(unitMatrix.rot);
            chunkData.chunk.ChunkHouseList ??= new List<ChunkHouseData>();
            chunkData.chunk.ChunkHouseList.Add(chunkHouse);
            mapEditor.SaveChunk(chunkData.chunk);
            var localPos = mapEditor.Map.transform.InverseTransformPoint(worldPosition);
            editorGos.AddUnitGo<EditorUnit>(chunkData.chunk.Id, mapEditor.Map.transform, unitMatrix.selectedPrefab, chunkHouse, localPos, unitMatrix.scale,
                    Quaternion.Euler(unitMatrix.rot));
        }

        public void ShowChunkView(AreaChunkData areaChunkData)
        {
            RemoveChunkView(areaChunkData);
            if (mapEditor.GetChunks()[areaChunkData.Id].ChunkHouseList == null)
                return;
            var list = mapEditor.GetChunks()[areaChunkData.Id].ChunkHouseList;
            foreach (var item in list)
            {
                var path = string.Format(ConstPath.HousePath, item.HouseId);
                editorGos.AddUnitGo<EditorUnit>(areaChunkData.Id, mapEditor.Map.transform, path, item, item.lPos, item.lScale, item.lRot);
            }
        }

        public void RemoveChunkView(AreaChunkData areaChunkData)
        {
            editorGos.RemoveChunk(areaChunkData.Id);
        }


        protected override void RemoveUnit(int chunkIndex, int index, bool isPortal)
        {
            mapEditor.GetChunks()[chunkIndex].ChunkHouseList.RemoveAt(index);
            for (int i = 0; i < mapEditor.GetChunks()[chunkIndex].ChunkHouseList.Count; i++)
            {
                mapEditor.GetChunks()[chunkIndex].ChunkHouseList[i].Index = i;
            }
        }

        protected override void NoSelectObj()
        {
            mapGoCreateEditor.NoSelectUnit();
        }
    }
}