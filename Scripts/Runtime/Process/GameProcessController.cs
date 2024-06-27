using GameFrame;

namespace GXGame
{
    public class GameProcessController : FsmController
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