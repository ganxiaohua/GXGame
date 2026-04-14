using Cysharp.Threading.Tasks;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class GameStartState : FsmState
    {
        public override void OnEnter()
        {
            base.OnEnter();
            LoadConfig().Forget();
        }

        private async UniTask LoadConfig()
        {
            var succ = await Tables.InitializeAsync();
            if (!succ)
                return;
            SceneFactory.ChangePlayerScene<GameScene>();
            GXGameFrame.Instance.RemoveFsmComponents(fsmController);
        }
    }
}