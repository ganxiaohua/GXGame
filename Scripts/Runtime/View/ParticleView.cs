using GameFrame;

namespace GXGame
{
    public class ParticleView : GameObjectView
    {
        private ECSEntity m_BindEntity;

        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load("Skill1Effect").Forget();
            m_BindEntity = ecsEntity;
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}