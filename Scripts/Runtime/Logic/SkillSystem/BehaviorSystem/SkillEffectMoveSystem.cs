using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class SkillEffectMoveSystem : IStartSystem<Context>, IUpdateSystem
    {
        private Group m_Group;

        public void Start(Context context)
        {
            Matcher matcher = Matcher.SetAll(Components.SkillComponent, Components.MoveDirection);
            m_Group = context.GetGroup(matcher);
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var enitiy in m_Group)
            {
                enitiy.SetMoveDirection(enitiy.GetMoveDirection().Dir);
            }
        }

        public void Clear()
        {
        }
    }
}