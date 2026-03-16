using System.Collections.Generic;
using GamePlay.Runtime;
using UnityEngine;

namespace GamePlay.Editor.Data
{
    public static class TableEditorConst
    {
        private static Dictionary<string, int> goNameWithIds;

        // private static Dictionary<int, string> modelIdWithEditorPath;

        private static Dictionary<int, List<int>> modelIDWithUnitIds;

        // private static void GetResPrefabs()
        // {
        //     modelIdWithEditorPath = new Dictionary<int, string>();
        //     var guids = AssetDatabase.FindAssets("t:Prefab", new string[] {"Assets/ResEditor/Prefabs"});
        //     foreach (var guid in guids)
        //     {
        //         var path = AssetDatabase.GUIDToAssetPath(guid);
        //         var name = YooAssetPath.GetAssetPath(path);
        //         var id = GetModelId(name, false);
        //         if (id == 0)
        //             continue;
        //         modelIdWithEditorPath[id] = path;
        //     }
        // }

        public static string GetEditorStr(int unitid)
        {
            // if (modelIdWithEditorPath == null)
            //     GetResPrefabs();
            var model = Tables.Instance.UnitTable.GetOrDefault(unitid).Model_Ref.Path;
            return model.Replace("Res", "ResEditor") + ".prefab";
        }

        public static int GetModelId(string prafabName, bool showDebug = true)
        {
            if (goNameWithIds == null)
            {
                var table = Tables.Instance.ModelTable;
                goNameWithIds = new();
                foreach (var dataItem in table.DataList)
                {
                    var ptah = dataItem.Path.Replace("Res", "ResEditor");
                    goNameWithIds[ptah] = dataItem.Id;
                }
            }

            return goNameWithIds.GetValueOrDefault(prafabName);
        }

        public static List<int> GetUnitIdsWithModelName(string prafabsName)
        {
            if (modelIDWithUnitIds == null)
            {
                modelIDWithUnitIds = new();
                var table = Tables.Instance.UnitTable;
                foreach (var unitItem in table.DataList)
                {
                    if (!modelIDWithUnitIds.TryGetValue(unitItem.Model, out var list))
                    {
                        list = new List<int>();
                        modelIDWithUnitIds[unitItem.Model] = list;
                    }

                    list.Add(unitItem.Id);
                }
            }

            var modelId = GetModelId(prafabsName);
            if (modelId == 0)
                return null;
            if (!modelIDWithUnitIds.TryGetValue(modelId, out var name))
            {
                Debug.LogError($"模型表配置了{modelId},但是单位表找不到这个模型的unit");
                return null;
            }

            return name;
        }
    }
}