using GameFrame.Runtime;

namespace GXGame
{
    public class GameInitState : FsmState
    {
        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            // Config.Instance.LoadTable();
            fsmController.ChangeState<GameStartState>();
        }
    }
}