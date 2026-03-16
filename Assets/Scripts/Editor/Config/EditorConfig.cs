using System.IO;
using UnityEditor;

namespace GameFrame.Editor
{
    public static class EditorConfig
    {
        public class ExcelLog : IShellLogHandler
        {
            public bool LogIsError(string log)
            {
                return log.Contains("|ERROR|");
            }

            public bool ErrorIsLog(string err)
            {
                return true;
            }
        }

        public static void BuildExacel()
        {
            try
            {
                EditorUtility.DisplayProgressBar($"Gen excel", "waiting...", 0f);
                string excelToolBatPath = EditorString.GetPath("ExcelToolBat");
                ShellHelper.Start(excelToolBatPath, null, Path.GetDirectoryName(excelToolBatPath), new ExcelLog());
                AssetDatabase.Refresh();
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }
        }
    }
}