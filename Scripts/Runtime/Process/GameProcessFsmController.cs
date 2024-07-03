using GameFrame;

namespace GXGame
{
    public class GameProcessFsmController : FsmController
    {
        public override void Start()
        {
            base.Start();
            AddState<GameInitState>();
            AddState<GameStartState>();
            SwitchState<GameInitState>();
        }
    }
}