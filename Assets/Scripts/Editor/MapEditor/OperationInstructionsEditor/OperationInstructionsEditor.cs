using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class OperationInstructionsEditor : OdinEditorWindow, IMapEditorSceneUpdate
    {
        public bool isClear { get; }

        [TextArea(5, 5)]
        [ReadOnly]
        [LabelText("操作说明")]
        public string txtArea;

        public static OperationInstructionsEditor Create()
        {
            var window = ScriptableObject.CreateInstance<OperationInstructionsEditor>();
            return window;
        }

        public void Init(MapEditor mapEditor)
        {
            txtArea = "1.点击区域配置的加载按钮\n" +
                      "2.选择物体放置，然后选择你想要放置的prafeb,选中之后放置在场景中\n" +
                      "3.左shift选中区域可以显示和隐藏物体\n" +
                      "4.alt按下可以对物体进行删除和其他操作" +
                      "5.ctrl+s保存";
        }

        public void Clear()
        {
        }

        public void OnSceneUpdate(SceneView view)
        {
        }
    }
}