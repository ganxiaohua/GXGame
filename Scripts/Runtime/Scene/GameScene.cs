using Cysharp.Threading.Tasks;
using GameFrame;
using GXGame.Logic;

namespace GXGame
{
    public class GameScene : Entity, IScene, IStartSystem, IUpdateSystem
    {
        public void Start()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            await AssetManager.Instance.LoadSceneAsync("Assets/GXGame/Scenes/Game.unity");
            AddComponent<GameWrold>();
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
        
        }

        public override void Clear()
        {
            AssetManager.Instance.DecrementReferenceCount("Assets/GXGame/Scenes/Game.unity");
            base.Clear();
        }
    }
}