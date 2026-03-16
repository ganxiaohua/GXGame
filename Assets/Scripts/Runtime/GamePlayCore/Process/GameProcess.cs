using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class GameProcess : FsmController
    {
        public override void OnInitialize()
        {
            base.OnInitialize();
            AddState<GameInitState>();
            AddState<GameStartState>();
            ChangeState<GameInitState>();
        }
    }
}