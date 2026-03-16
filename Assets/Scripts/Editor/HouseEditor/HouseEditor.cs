using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;

namespace GamePlay.Editor.MapEditor
{
    public partial class HouseEditor : OdinMenuEditorWindow
    {
        private OdinMenuTree menuTree;
        private IHouseEditorSceneUpdate curSelect;

        [MenuItem("GX框架工具/地图编辑器/内空间编辑器")]
        public static void Create()
        {
            var mapEditor = GetWindow<HouseEditor>();
            mapEditor.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            titleContent.text = "内空间编辑器";
            menuTree = new OdinMenuTree(supportsMultiSelect: false)
            {
                    {"内空间编辑", HouseCreateGoEditor.Create(), SdfIconType.Controller},
            };
            UnityEditor.SceneView.duringSceneGui += OnSceneUpdate;
            menuTree.Selection.SelectionChanged += OnSelect;
            return menuTree;
        }

        protected override void OnDestroy()
        {
            UnityEditor.SceneView.duringSceneGui -= OnSceneUpdate;
            foreach (var item in menuTree.MenuItems)
            {
                ((IHouseEditorSceneUpdate) (item.Value)).Clear();
            }

            base.OnDestroy();
            ToolbarExtensions.OnGoInitSceneNoSave();
        }


        private void OnSelect(SelectionChangedType type)
        {
            if (type == SelectionChangedType.SelectionCleared)
            {
                if (curSelect != null)
                    curSelect.Clear();
            }
            else if (type == SelectionChangedType.ItemAdded)
            {
                var selectedValue = menuTree.Selection.SelectedValue;
                if (selectedValue != null && (curSelect == null || (curSelect != null && curSelect != selectedValue)))
                {
                    curSelect = (IHouseEditorSceneUpdate) selectedValue;
                    curSelect.Init(this);
                }
            }
        }

        private void OnSceneUpdate(SceneView view)
        {
            if (curSelect == null)
                return;
            curSelect.OnSceneUpdate(view);
        }
    }
}