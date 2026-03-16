using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class OperationInstructionsRuntimeEditor : OdinEditorWindow
    {
        public bool isClear { get; }

        [TextArea(10, 20)]
        [LabelText("操作说明")]
        public string txtArea;

        public static OperationInstructionsRuntimeEditor Create()
        {
            var window = ScriptableObject.CreateInstance<OperationInstructionsRuntimeEditor>();
            window.Show();
            window.minSize = new Vector2(600, 500);
            window.Init();
            return window;
        }

        public void Init()
        {
            txtArea = "0.进入工程搜索main.scene 点击开始游戏\n" +
                      "1.wasd 控制移动\n" +
                      "2.tab键呼出背包\n" +
                      "3.滚动滚轮中间键选择道具\n" +
                      "4.按住cltrl+滚动中间键调整相机\n" +
                      "5.鼠标左键进行操作(交互物品需要用不同的道具操作\n" +
                      "6.鼠标右键是控制人物同一个方向行走\n" +
                      "7.F键投掷物品\n" +
                      "8.shift加速奔跑";
        }

        public void Clear()
        {
        }

        public void OnSceneUpdate(SceneView view)
        {
        }
    }
}