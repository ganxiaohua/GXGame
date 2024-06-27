using GameFrame;

namespace GXGame
{
    public class GameInitState : FsmState
    {
        public override void Enter(FsmController fsmController)
        {
            base.Enter(fsmController);
            Config.Instance.LoadTable();
            fsmController.SwitchState<GameStartState>();
        }
    }
}