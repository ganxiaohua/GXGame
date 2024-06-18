using GameFrame;

namespace GXGame
{
    public class ParticleView : GameObjectView
    {

        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load("Skill1Effect").Forget();
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}