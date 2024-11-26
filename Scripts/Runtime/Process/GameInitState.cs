using GameFrame;

namespace GXGame
{
    public class GameInitState : FsmState
    {
        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            // Config.Instance.LoadTable();
            //播放一个icon之类
            fsmController.ChangeState<GameStartState>();
        }
    }
}