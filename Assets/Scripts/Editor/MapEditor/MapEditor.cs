using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class MapEditor : OdinMenuEditorWindow
    {
        private OdinMenuTree menuTree;
        private IMapEditorSceneUpdate curSelect;

        [MenuItem("GX框架工具/地图编辑器/大地图")]
        public static void Create()
        {
            var mapEditor = GetWindow<MapEditor>();
            mapEditor.Show();
        }

        protected override OdinMenuTree BuildMenuTree()
        {
            titleContent.text = "地图编辑器";
            menuTree = new OdinMenuTree(supportsMultiSelect: false)
            {
                    {"区域配置", RegionalDivisionEditor.Create(), SdfIconType.Cloudy},
                    {"物体放置", MapGoCreateEditor.Create(), SdfIconType.Controller},
                    {"耕地设置", ArableLandEditor.Create(), SdfIconType.Crop},
                    {"操作说明", OperationInstructionsEditor.Create(), SdfIconType.Book}
            };
            UnityEditor.SceneView.duringSceneGui += OnSceneUpdate;
            menuTree.Selection.SelectionChanged += OnSelect;
            // TrySelectMenuItemWithObject(menuTree.MenuItems[0].Value);
            return menuTree;
        }

        protected override void OnDestroy()
        {
            UnityEditor.SceneView.duringSceneGui -= OnSceneUpdate;
            foreach (var item in menuTree.MenuItems)
            {
                ((IMapEditorSceneUpdate) (item.Value)).Clear();
            }

            if (Map != null)
                GameObject.DestroyImmediate(Map);
            AreaData = null;
            ToolbarExtensions.OnGoInitSceneNoSave();
            base.OnDestroy();
        }


        private void OnSelect(SelectionChangedType type)
        {
            if (type == SelectionChangedType.SelectionCleared)
            {
                if (curSelect != null)
                {
                    curSelect.Clear();
                    curSelect = null;
                }
            }
            else if (type == SelectionChangedType.ItemAdded)
            {
                var selectedValue = menuTree.Selection.SelectedValue;
                if (selectedValue != null && (curSelect == null || (curSelect != null && curSelect != selectedValue)))
                {
                    curSelect = (IMapEditorSceneUpdate) selectedValue;
                    curSelect.Init(this);
                }
            }
        }

        private void OnSceneUpdate(SceneView view)
        {
            if (curSelect == null)
                return;
            ShowArea();
            curSelect.OnSceneUpdate(view);
        }
    }
}