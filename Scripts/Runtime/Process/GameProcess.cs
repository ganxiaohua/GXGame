using GameFrame;

namespace GXGame
{
    public class GameProcess : FsmController
    {
        public override void Initialize()
        {
            base.Initialize();
            AddState<GameInitState>();
            AddState<GameStartState>();
            ChangeState<GameInitState>();
        }
    }
}