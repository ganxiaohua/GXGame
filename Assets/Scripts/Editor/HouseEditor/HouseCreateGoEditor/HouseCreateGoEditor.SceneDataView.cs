using GameFrame.Editor;
using GamePlay.Editor.Data;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class HouseCreateGoEditor
    {
        private void InjectGoView(Vector3 worldPosition, Unit unit)
        {
            if (houseEditor.House == null)
            {
                EditorUtility.DisplayDialog(
                        "提示",
                        "area数据为空，你必须加载一张地图。",
                        "确定"
                );
                return;
            }

            if (unitMatrix.selectedPrefab == null)
            {
                EditorUtility.DisplayDialog(
                        "提示",
                        "选中物体为空",
                        "确定");
                return;
            }

            if (unitMatrix.selectUnitId == 0)
            {
                EditorUtility.DisplayDialog(
                        "提示",
                        "请选择一个UnitId",
                        "确定");
                return;
            }

            var name = YooAssetPath.GetAssetPath(AssetDatabase.GetAssetPath(unitMatrix.selectedPrefab));
            int id = TableEditorConst.GetModelId(name);
            if (id == 0)
            {
                return;
            }

            var localPos = houseEditor.House.transform.InverseTransformPoint(worldPosition);
            if (unit is not PortalUnit)
                editorGos.AddUnitGo<EditorUnit>(0, houseEditor.House.transform, unitMatrix.selectedPrefab, unit, localPos, unitMatrix.scale,
                        Quaternion.Euler(unitMatrix.rot));
            else
                editorGos.AddUnitGo<PortalEditorUnit>(0, houseEditor.House.transform, unitMatrix.selectedPrefab, unit, localPos, unitMatrix.scale,
                        Quaternion.Euler(unitMatrix.rot));
        }

        private void CreateUnit(Unit unit)
        {
            var path = TableEditorConst.GetEditorStr(unit.UnitId);
            if (unit is not PortalUnit)
                editorGos.AddUnitGo<EditorUnit>(0, houseEditor.House.transform, path, unit, unit.lPos, unit.lScale, unit.lRot);
            else
            {
                editorGos.AddUnitGo<PortalEditorUnit>(0, houseEditor.House.transform, path, unit, unit.lPos, unit.lScale, unit.lRot);
            }
        }
    }
}