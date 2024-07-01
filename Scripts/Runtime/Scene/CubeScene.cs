using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public class CubeScene : Entity, IScene, IStartSystem, IUpdateSystem
    {
        private BehaviorTest bt;
        public void Start()
        {
            //ecs demo
            Init().Forget();
            //行为机 demo
            bt = new BehaviorTest();
            bt.Init(this);
        }

        private async UniTaskVoid Init()
        {
            await AssetManager.Instance.LoadSceneAsync("Assets/GXGame/Scenes/CubeScene.unity");
            AddComponent<CubeConText>();
            UIManager.Instance.OpenUI(typeof(UICardListWindow));
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            bt?.Update(elapseSeconds,realElapseSeconds);
        }

        public override void Clear()
        {
            AssetManager.Instance.DecrementReferenceCount("Assets/GXGame/Scenes/CubeScene.unity");
            base.Clear();
        }
    }
}