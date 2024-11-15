using Cysharp.Threading.Tasks;
using GameFrame;
using GXGame.Logic;

namespace GXGame
{
    public class GameScene : Entity, IScene, IInitializeSystem, IUpdateSystem
    {
        public void Initialize()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init()
        {
            await AssetManager.Instance.LoadSceneAsync("Assets/GXGame/Scenes/Game.unity");
            AddComponent<GameWrold>();
            // AddComponent<UIObjectData>().Data = "xxxxxxxxxxx";
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        
        }

        public override void Dispose()
        {
            AssetManager.Instance.DecrementReferenceCount("Assets/GXGame/Scenes/Game.unity");
            base.Dispose();
        }
    }
}