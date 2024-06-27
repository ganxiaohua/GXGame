using GameFrame;

namespace GXGame
{
    public class GameStartState : FsmState
    {
        public override void Enter(FsmController fsmController)
        {
            base.Enter(fsmController);
            SceneFactory.ChangePlayerScene<CubeScene>(this);
        }
    }
}