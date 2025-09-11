using GameFrame.Runtime;

namespace GXGame
{
    public class GameStartState : FsmState
    {
        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            SceneFactory.ChangePlayerScene<GameScene>();
            GXGameFrame.Instance.RemoveFsmComponents(fsmController);
        }
    }
}