using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class RegionalDivisionEditor : OdinEditorWindow, IMapEditorSceneUpdate
    {
        private static RegionalDivisionEditor window;
        public bool isClear { get; set; }
        private MapEditor mapEditor;

        public static RegionalDivisionEditor Create()
        {
            window = ScriptableObject.CreateInstance<RegionalDivisionEditor>();
            return window;
        }

        public void Init(MapEditor mapEditor)
        {
            this.mapEditor = mapEditor;
        }


        public void Clear()
        {
        }

        public void OnSceneUpdate(SceneView view)
        {
        }
    }
}