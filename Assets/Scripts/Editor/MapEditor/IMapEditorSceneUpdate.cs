namespace GamePlay.Editor.MapEditor
{
    public interface IMapEditorSceneUpdate
    {
        public void Init(MapEditor mapEditor);

        public void Clear();

        public bool isClear { get; }

        void OnSceneUpdate(UnityEditor.SceneView view);
    }
}