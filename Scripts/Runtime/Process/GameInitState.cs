using GameFrame;

namespace GXGame
{
    public class GameInitState : FsmState
    {
        public override void Enter(FsmController fsmController)
        {
            base.Enter(fsmController);
            // Config.Instance.LoadTable();
            //播放一个icon之类
            fsmController.SwitchState<GameStartState>();
        }
    }
}