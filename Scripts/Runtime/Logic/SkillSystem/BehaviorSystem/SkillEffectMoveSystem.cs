using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class SkillEffectMoveSystem : IStartSystem<World>, IUpdateSystem
    {
        private Group m_Group;

        public void Start(World world)
        {
            Matcher matcher = Matcher.SetAll(Components.SkillComponent, Components.MoveDirection);
            m_Group = world.GetGroup(matcher);
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in m_Group)
            {
                entity.SetMoveDirection(entity.GetMoveDirection().Dir);
            }
        }

        public void Clear()
        {
        }
    }
}