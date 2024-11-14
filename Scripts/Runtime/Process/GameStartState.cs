using GameFrame;

namespace GXGame
{
    public class GameStartState : FsmState
    {
        public override void OnEnter(FsmController fsmController)
        {
            base.OnEnter(fsmController);
            SceneFactory.ChangePlayerScene<GameScene>(this);
        }
    }
}