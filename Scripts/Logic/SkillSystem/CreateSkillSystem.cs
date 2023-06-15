using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class CreateSkillSystem : IECSStartSystem
    {
        public void Start(Context context)
        {
            Matcher matcher = Matcher.SetAllOfIndices(Components.SkillGroupComponent);
            Group group = context.GetGroup(matcher);
            foreach (var entities in group.EntitiesMap)
            {
                foreach (var id in entities.GetSkillGroupComponent().IDs)
                {
                    SkillEntity entity =  context.AddChild<SkillEntity>();
                    entity.AddSkillIDComponent(id);
                }
            }
        }

        public void Clear()
        {
            throw new System.NotImplementedException();
        }
    }
}