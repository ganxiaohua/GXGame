using System.Collections.Generic;
using GameFrame.Editor;
using GamePlay.Editor.Data;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    [HideReferenceObjectPicker]
    public class UnitCreateEditor : CreateEditorBase
    {
        [ShowInInspector] [PropertyOrder(1)]
        private FolderMatrix folderMatrix;

        public override void Init(MapEditor mapEditor, MapGoCreateEditor mapGoCreateEditor)
        {
            base.Init(mapEditor, mapGoCreateEditor);
            folderMatrix ??= new FolderMatrix();
            folderMatrix.InitPrefabsFolders(MapGoCreateData.ResMapEditorData, OnFolderMatrix);
        }

        public override void Clear()
        {
            base.Clear();
            folderMatrix?.ClearSelect(MapGoCreateData.ResMapEditorData);
            folderMatrix = null;
        }

        protected override void InjectGo(Vector3 worldPosition)
        {
            var name = YooAssetPath.GetAssetPath(AssetDatabase.GetAssetPath(unitMatrix.selectedPrefab));
            int id = TableEditorConst.GetModelId(name);
            if (id == 0)
            {
                return;
            }

            var item = Tables.Instance.UnitTable.Get(unitMatrix.selectUnitId);
            if (item.GetItem().ItemType != ItemType.Portal)
                CreateCommonUnit(worldPosition);
            else
            {
                CreatePortal(worldPosition);
            }
        }

        private void CreateCommonUnit(Vector3 worldPosition)
        {
            var chunkData = mapEditor.GetChunkWithPos(worldPosition);
            var unit = new Unit()
            {
                    Index = chunkData.chunk.Units.Count,
                    UnitId = unitMatrix.selectUnitId,
                    lPos = worldPosition,
                    lScale = unitMatrix.scale,
                    lRot = Quaternion.Euler(unitMatrix.rot)
            };
            chunkData.chunk.Units.Add(unit);
            var localPos = mapEditor.Map.transform.InverseTransformPoint(worldPosition);
            editorGos.AddUnitGo<EditorUnit>(chunkData.chunk.Id, mapEditor.Map.transform, unitMatrix.selectedPrefab, unit, localPos, unitMatrix.scale,
                    Quaternion.Euler(unitMatrix.rot));
        }

        private void CreatePortal(Vector3 worldPosition)
        {
            var chunkData = mapEditor.GetChunkWithPos(worldPosition);
            var unit = new PortalUnit()
            {
                    Index = chunkData.chunk.Units.Count,
                    UnitId = unitMatrix.selectUnitId,
                    lPos = worldPosition,
                    lScale = unitMatrix.scale,
                    lRot = Quaternion.Euler(unitMatrix.rot),
                    PortaTagertlID = 1,
                    ProtalTargetPoint = new Vector3(0, 0, 0)
            };
            if (chunkData.chunk.ProtalUnit == null)
                chunkData.chunk.ProtalUnit = new List<PortalUnit>();
            chunkData.chunk.ProtalUnit.Add(unit);
            var localPos = mapEditor.Map.transform.InverseTransformPoint(worldPosition);
            editorGos.AddUnitGo<PortalEditorUnit>(chunkData.chunk.Id, mapEditor.Map.transform, unitMatrix.selectedPrefab, unit, localPos, unitMatrix.scale,
                    Quaternion.Euler(unitMatrix.rot));
        }

        public void ShowChunkView(AreaChunkData areaChunkData)
        {
            RemoveChunkView(areaChunkData);
            var data = mapEditor.GetChunks()[areaChunkData.Id];
            if (data.Units == null)
                return;
            foreach (var item in data.Units)
            {
                var path = TableEditorConst.GetEditorStr(item.UnitId);
                editorGos.AddUnitGo<EditorUnit>(areaChunkData.Id, mapEditor.Map.transform, path, item, item.lPos, item.lScale, item.lRot);
            }

            if (data.ProtalUnit == null)
                return;
            foreach (var item in data.ProtalUnit)
            {
                var path = TableEditorConst.GetEditorStr(item.UnitId);
                editorGos.AddUnitGo<PortalEditorUnit>(areaChunkData.Id, mapEditor.Map.transform, path, item, item.lPos, item.lScale, item.lRot);
            }
        }

        public void RemoveChunkView(AreaChunkData areaChunkData)
        {
            editorGos.RemoveChunk(areaChunkData.Id);
        }

        private void OnFolderMatrix(HashSet<string> data)
        {
            unitMatrix.SetDrawData(data);
        }

        protected override void RemoveUnit(int chunkId, int index, bool isportal)
        {
            if (!isportal)
            {
                mapEditor.GetChunks()[chunkId].Units.RemoveAt(index);
                for (int i = 0; i < mapEditor.GetChunks()[chunkId].Units.Count; i++)
                {
                    mapEditor.GetChunks()[chunkId].Units[i].Index = i;
                }
            }
            else
            {
                mapEditor.GetChunks()[chunkId].ProtalUnit.RemoveAt(index);
                for (int i = 0; i < mapEditor.GetChunks()[chunkId].ProtalUnit.Count; i++)
                {
                    mapEditor.GetChunks()[chunkId].ProtalUnit[i].Index = i;
                }
            }
        }

        protected override void NoSelectObj()
        {
            mapGoCreateEditor.NoSelectHouse();
        }
    }
}