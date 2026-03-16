namespace GamePlay.Editor.MapEditor
{
    public interface IHouseEditorSceneUpdate
    {
        public void Init(HouseEditor mapEditor);

        public void Clear();

        public bool isClear { get; }

        void OnSceneUpdate(UnityEditor.SceneView view);
    }
}