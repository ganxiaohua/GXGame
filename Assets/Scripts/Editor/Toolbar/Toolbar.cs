using GameFrame.Editor;
using GamePlay.Editor.MapEditor;
using UnityEditor;

namespace GamePlay.Editor
{
    [InitializeOnLoad]
    public static class ToolbarExtensions
    {
        static ToolbarExtensions()
        {
            GameFrame.Editor.ToolbarExtensions.RegisterRightButton("绑定脚本", AutoCreate.AutoAllScript);
            GameFrame.Editor.ToolbarExtensions.RegisterRightButton("回到初始场景", OnGoInitScene);
            // GameFrame.Editor.ToolbarExtensions.RegisterLeftButton("打开Excels", OnOpenExcel);
            // GameFrame.Editor.ToolbarExtensions.RegisterLeftButton("编译Excels", EditorConfig.BuildExacel);
            // GameFrame.Editor.ToolbarExtensions.RegisterLeftButton("地图编辑器", () => { GamePlay.Editor.MapEditor.MapEditor.Create(); });
            // GameFrame.Editor.ToolbarExtensions.RegisterLeftButton("内空间编辑器", () => { GamePlay.Editor.MapEditor.HouseEditor.Create(); });
            GameFrame.Editor.ToolbarExtensions.RegisterLeftButton("操作说明", () => { OperationInstructionsRuntimeEditor.Create(); });
        }

        private static void OnOpenExcel()
        {
            EditorUtility.RevealInFinder("../Excel/Datas/");
        }

        public static void OnGoInitScene()
        {
            EditorCommon.GoScene("Assets/Res/Scenes/Main.unity");
        }

        public static void OnGoInitSceneNoSave()
        {
            EditorCommon.GoSceneNoSave("Assets/Res/Scenes/Main.unity");
        }
    }
}