using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public class CubeScene : Entity, IScene,IStartSystem, IUpdateSystem, IClearSystem
    {
        public void Start()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            await AssetSystem.Instance.LoadSceneAsync("Assets/GXGame/Scenes/CubeScene.unity");
            AddComponent<CubeConText>();
            UIManager.Instance.OpenUI(typeof(UICardListWindow));
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
        }
        
        public override void Clear()
        {
            base.Clear();
            AssetSystem.Instance.DecrementReferenceCount("Assets/GXGame/Scenes/CubeScene.unity");
        }
    }
}