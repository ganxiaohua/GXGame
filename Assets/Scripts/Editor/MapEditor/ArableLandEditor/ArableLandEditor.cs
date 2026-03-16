using System.Collections.Generic;
using System.IO;
using GameFrame.Editor;
using GamePlay.Editor.MapEditor;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using OdinEditorWindow = Sirenix.OdinInspector.Editor.OdinEditorWindow;

namespace GamePlay.Editor
{
    public partial class ArableLandEditor : OdinEditorWindow, IMapEditorSceneUpdate
    {
        public bool isClear { get; private set; }

        private MapEditor.MapEditor editor;

        private List<GameObject> cropLandList;

        [SerializeField]
        public Vector2 CellSize = new Vector2(1, 1);

        [SerializeField]
        public Vector2Int GirdArea = new Vector2Int(10, 10);

        private Dictionary<int, AreaChunkData> areaChunkDataDic;

        private int curMaxIndex = -1;

        [ShowInInspector] [LabelText("开始配置")]
        private bool IsConfiguration;

        public static ArableLandEditor Create()
        {
            var window = ScriptableObject.CreateInstance<ArableLandEditor>();
            return window;
        }

        public void Init(MapEditor.MapEditor mapEditor)
        {
            this.editor = mapEditor;
            cropLandList = new List<GameObject>();
            areaChunkDataDic = new();
            Load();
            isClear = false;
        }

        public void Clear()
        {
            if (isClear)
                return;
            Save();
            if (GridManager.instance.GridDataList != null)
                GridManager.instance.GridDataList.Clear();
            IsConfiguration = false;
            if (cropLandList != null)
                foreach (var item in cropLandList)
                {
                    if (item != null)
                    {
                        GameObject.DestroyImmediate(item);
                    }
                }

            isClear = true;
        }

        public void OnSceneUpdate(SceneView view)
        {
            if (!IsConfiguration || editor.AreaData == null)
                return;
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                Handles.Label(hit.point, hit.point.ToString() + "点击创建", GuiStyle.GetComGuiStytle());
                if (e.type == EventType.MouseDown && e.button == 0)
                {
                    CreateCropData(hit.point);
                    e.Use();
                }
            }
        }

        private void CreateCropData(Vector3 pos)
        {
            CroplandDataBase cdb = new CroplandDataBase();
            cdb.CellSize = CellSize;
            cdb.GirdArea = GirdArea;
            cdb.lPos = pos;
            cdb.lRot = Quaternion.identity;
            cdb.lScale = Vector3.one;
            cdb.Index = ++curMaxIndex;
            CroplandData crop = ScriptableObject.CreateInstance<CroplandData>();
            crop.Data = cdb;
            if (editor.AreaData.CropLand != null)
            {
                if (!editor.AreaData.CropLand.Contains(curMaxIndex))
                    editor.AreaData.CropLand.Add(curMaxIndex);
                EditorUtility.SetDirty(editor.AreaData);
            }

            string path = string.Format(ConstPath.MapCropLandPath, editor.AreaData.Id, curMaxIndex);
            OpFile.CreateDirectory(Path.GetDirectoryName(path));
            AssetDatabase.CreateAsset(crop, path);
            var areaChunkData = GetChunkWihtPos(pos);
            if (areaChunkData.CroplandData != null)
            {
                if (!areaChunkData.CroplandData.Contains(curMaxIndex))
                    areaChunkData.CroplandData.Add(curMaxIndex);
                EditorUtility.SetDirty(areaChunkData);
            }
        }

        private void OnDestroyUnit(BaseRST bae, GameObject go)
        {
            if (cropLandList != null)
                cropLandList.Remove(go);
            if (editor.AreaData.CropLand != null)
            {
                editor.AreaData.CropLand.Remove(bae.Index);
                EditorUtility.SetDirty(editor.AreaData);
            }

            var areaChunkData = GetChunkWihtPos(go.transform.position);
            if (areaChunkData.CroplandData != null)
            {
                areaChunkData.CroplandData.Remove(bae.Index);
                EditorUtility.SetDirty(areaChunkData);
            }

            string assetPath = string.Format(ConstPath.MapCropLandPath, editor.AreaData.Id, bae.Index);
            var data = AssetDatabase.DeleteAsset(assetPath);
            GameObject.DestroyImmediate(go);
            AssetDatabase.Refresh();
        }

        private AreaChunkData GetChunkWihtPos(Vector3 pos)
        {
            var index = editor.GetChunkIndexWithPos(pos);

            if (!areaChunkDataDic.TryGetValue(index, out var areaChunkData))
            {
                string path = string.Format(ConstPath.MapChunkPath, editor.AreaData.Id, index);
                areaChunkData = AssetDatabase.LoadAssetAtPath<AreaChunkData>(path);
                areaChunkDataDic.Add(index, areaChunkData);
            }

            return areaChunkData;
        }


        [Button("保存")]
        private void Save()
        {
            if (cropLandList == null || cropLandList.Count == 0 || editor.AreaData == null)
                return;
            for (int i = 0; i < cropLandList.Count; i++)
            {
                var gridData = cropLandList[i].GetComponent<GridData>();
                string assetPath = string.Format(ConstPath.MapCropLandPath, editor.AreaData.Id, gridData.CroplandData.Index);
                var cropladndData = AssetDatabase.LoadAssetAtPath<CroplandData>(assetPath);
                cropladndData.Data.lPos = gridData.transform.position;
                cropladndData.Data.lRot = gridData.transform.rotation;
                cropladndData.Data.lScale = gridData.transform.localScale;
                cropladndData.Data.CellSize = gridData.CroplandData.CellSize;
                cropladndData.Data.GirdArea = gridData.CroplandData.GirdArea;
                EditorUtility.SetDirty(cropladndData);
            }

            // AssetDatabase.SaveAssets();
        }

        private void Load()
        {
            if (editor.AreaData == null || editor.AreaData.CropLand == null)
                return;
            foreach (var id in editor.AreaData.CropLand)
            {
                string assetPath = string.Format(ConstPath.MapCropLandPath, editor.AreaData.Id, id);
                var data = AssetDatabase.LoadAssetAtPath<CroplandData>(assetPath);
                GameObject go = new GameObject();
                go.transform.position = data.Data.lPos;
                go.transform.rotation = data.Data.lRot;
                var gridData = go.AddComponent<GridData>();
                gridData.CroplandData.GirdArea = data.Data.GirdArea;
                gridData.CroplandData.CellSize = data.Data.CellSize;
                gridData.CroplandData.Index = data.Data.Index;
                GridManager.instance.GridDataList.Add(gridData);
                cropLandList.Add(go);
                if (curMaxIndex < data.Data.Index)
                    curMaxIndex = data.Data.Index;
                var editorUnit = go.AddComponent<EditorUnit>();
                editorUnit.Init(data.Data, OnDestroyUnit);
            }
        }
    }
}