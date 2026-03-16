using System.Collections.Generic;
using System.IO;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GamePlay.Editor.MapEditor
{
    public partial class HouseCreateGoEditor
    {
        [HorizontalGroup("加载内空间")] [LabelText("房间Id")] [ShowInInspector]
        public int houseId;

        [HorizontalGroup("加载内空间")]
        [Button("加载内空间")]
        [ShowInInspector]
        public void LoadHouse()
        {
            Freed();
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


            string path = string.Format(ConstPath.HousePath, houseId);
            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog(
                        "没有这个地图",
                        $"{path}不存在，你必须先建立一个地基，存放在Res/Prefabs/House下",
                        "确定"
                );
                return;
            }

            var map = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            houseEditor.House = (GameObject) PrefabUtility.InstantiatePrefab(map);
            LoadHouseAsset();
        }

        private void LoadHouseAsset()
        {
            string path = string.Format(ConstPath.HouseAssetPath, houseId);
            if (!File.Exists(path))
            {
                houseEditor.HouseDataData = ScriptableObject.CreateInstance<HouseData>();
                houseEditor.HouseDataData.Id = houseId;
                AssetDatabase.CreateAsset(houseEditor.HouseDataData, path);
            }
            else
            {
                houseEditor.HouseDataData = AssetDatabase.LoadAssetAtPath<HouseData>(path);
                foreach (var unit in houseEditor.HouseDataData.Units)
                {
                    CreateUnit(unit);
                }

                foreach (var unit in houseEditor.HouseDataData.ProtalUnit)
                {
                    CreateUnit(unit);
                }
            }
        }

        private void InjectGo(Vector3 worldPosition)
        {
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
            houseEditor.HouseDataData.Units ??= new List<Unit>();
            var unit = new Unit()
            {
                    Index = houseEditor.HouseDataData.Units.Count,
                    UnitId = unitMatrix.selectUnitId,
                    lPos = worldPosition,
                    lScale = unitMatrix.scale,
                    lRot = Quaternion.Euler(unitMatrix.rot)
            };
            houseEditor.HouseDataData.Units.Add(unit);
            InjectGoView(worldPosition, unit);
        }

        private void CreatePortal(Vector3 worldPosition)
        {
            houseEditor.HouseDataData.ProtalUnit ??= new List<PortalUnit>();
            var unit = new PortalUnit()
            {
                    Index = houseEditor.HouseDataData.ProtalUnit.Count,
                    UnitId = unitMatrix.selectUnitId,
                    lPos = worldPosition,
                    lScale = unitMatrix.scale,
                    lRot = Quaternion.Euler(unitMatrix.rot),
                    PortaTagertlID = 1,
                    ProtalTargetPoint = new Vector3(0, 0, 0)
            };
            if (houseEditor.HouseDataData.ProtalUnit == null)
            {
                houseEditor.HouseDataData.ProtalUnit = new List<PortalUnit>();
            }

            houseEditor.HouseDataData.ProtalUnit.Add(unit);
            InjectGoView(worldPosition, unit);
        }


        private void RemoveUnit(int chunkId, int index, bool portal)
        {
            if (!portal)
            {
                houseEditor.HouseDataData.Units.RemoveAt(index);
                for (int i = 0; i < houseEditor.HouseDataData.Units.Count; i++)
                {
                    houseEditor.HouseDataData.Units[i].Index = i;
                }
            }
            else
            {
                houseEditor.HouseDataData.ProtalUnit.RemoveAt(index);
                for (int i = 0; i < houseEditor.HouseDataData.ProtalUnit.Count; i++)
                {
                    houseEditor.HouseDataData.ProtalUnit[i].Index = i;
                }
            }
        }
    }
}