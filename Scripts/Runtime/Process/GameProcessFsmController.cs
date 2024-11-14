using GameFrame;

namespace GXGame
{
    public class GameProcessFsmController : FsmController
    {
        public override void Initialize()
        {
            base.Initialize();
            AddState<GameInitState>();
            AddState<GameStartState>();
            SwitchState<GameInitState>();
        }
    }
}