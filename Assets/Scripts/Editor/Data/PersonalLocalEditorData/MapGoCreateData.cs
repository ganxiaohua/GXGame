using System.Collections.Generic;
using UnityEditor;

namespace GamePlay.Editor
{
    public static class MapGoCreateData
    {
        public static string ResMapEditorData = "MapGoCreateData_ResMapEditorData1";

        public static string ResHouseEditorData = "MapGoCreateData_ResHouseEditorData1";

        [MenuItem("GX框架工具/EditorPrefs/清理MapGoCreateData editorPrefs")]
        public static void Clear()
        {
            EditorPrefs.DeleteKey(ResMapEditorData);
            EditorPrefs.DeleteKey(ResHouseEditorData);
        }

        public static void SetSelectResEditorPaths(HashSet<string> paths, string key)
        {
            string str = null;
            foreach (var path in paths)
            {
                str += path + ';';
            }

            if (!string.IsNullOrEmpty(str))
                EditorPrefs.SetString(key, str.Substring(0, str.Length - 1));
        }

        public static string[] GetSelectResEditorPaths(string key)
        {
            string str = EditorPrefs.GetString(key);
            if (string.IsNullOrEmpty(str))
                return null;
            var paths = str.Split(';');
            return paths;
        }
    }
}