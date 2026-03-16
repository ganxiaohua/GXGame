using GameFrame.Editor;
using UnityEditor;

namespace GamePlay.Editor
{
    public partial class GamePlayEditorMenu
    {
        [MenuItem("GX框架工具/Config/Build Excel", false, 0)]
        public static void BuildPb()
        {
            EditorConfig.BuildExacel();
        }
    }
}