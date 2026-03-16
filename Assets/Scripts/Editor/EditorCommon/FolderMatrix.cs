using System;
using System.Collections.Generic;
using GameFrame.Editor;
using GamePlay.Editor.MapEditor;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    [HideReferenceObjectPicker]
    public class FolderMatrix
    {
        [TableMatrix(HorizontalTitle = "选择文件夹", RowHeight = 20, DrawElementMethod = "DrawColoredEnumElementString")] [ShowInInspector] [PropertyOrder(1)]
        private string[,] prefabsFolders;

        private HashSet<string> showPaths;
        private readonly int stringLine = 10;
        private Action<HashSet<string>> action;

        public void InitPrefabsFolders(string key, Action<HashSet<string>> action)
        {
            this.action = action;
            showPaths = new();
            var paths = OpFile.GetSubDirectories(MapDataConst.PrefabsPath);
            prefabsFolders = new string[stringLine, paths.Length / stringLine + paths.Length % stringLine == 0 ? 0 : 1];
            for (int i = 0; i < paths.Length; i++)
            {
                prefabsFolders[i % stringLine, i / stringLine] = paths[i].Replace(MapDataConst.PrefabsPath, "").Replace("\\", "");
            }

            var selectPaths = MapGoCreateData.GetSelectResEditorPaths(key);
            if (selectPaths != null && selectPaths.Length > 0)
                showPaths.AddRange(selectPaths);
            if (showPaths.Count != 0)
                Refresh();
        }


        public void ClearSelect(string key)
        {
            prefabsFolders = null;
            if (showPaths == null)
                return;
            MapGoCreateData.SetSelectResEditorPaths(showPaths, key);
            showPaths = null;
        }

        private string DrawColoredEnumElementString(Rect rect, string str)
        {
            var lasetStr = $"{MapDataConst.PrefabsPath}/{str}";
            bool select = showPaths.Contains(lasetStr);
            // 处理鼠标点击事件
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                bool succ = showPaths.Add(lasetStr);
                if (succ)
                    select = true;
                else
                {
                    select = false;
                    showPaths.Remove(lasetStr);
                }
            }

            if (select)
            {
                Color oldColor = GUI.backgroundColor;
                GUI.backgroundColor = new Color(0.3f, 0.1f, 0.1f);
                EditorGUI.DrawRect(rect, GUI.backgroundColor);
                GUI.backgroundColor = oldColor;
            }

            EditorGUI.LabelField(rect, str, GuiStyle.GetComGuiStytle());
            return str;
        }

        [Button("设置预览")]
        [PropertyOrder(2)]
        public void Refresh()
        {
            action?.Invoke(showPaths);
        }
    }
}