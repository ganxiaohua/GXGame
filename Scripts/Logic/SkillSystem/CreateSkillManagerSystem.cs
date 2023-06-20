using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class CreateSkillManagerSystem : IECSStartSystem
    {
        public void Start(Context context)
        {
            Matcher matcher = Matcher.SetAllOfIndices(Components.SkillGroupComponent);
            Group group = context.GetGroup(matcher);
            foreach (var entitie in group.EntitiesMap)
            {
                foreach (var id in entitie.GetSkillGroupComponent().IDs)
                {
                    SkillManagerEntity managerEntity = context.AddChild<SkillManagerEntity>();
                    managerEntity.AddSkillIDComponent(id);
                    managerEntity.AddSkillManagerStateComponent(SkillManagerState.None);
                    managerEntity.AddOnSpellStartComponent(KeyCode.K);
                    managerEntity.AddSkillOwnerComponent(entitie);
                    managerEntity.AddAbilityCooldownComponent(5);
                    managerEntity.AddAbilityCastRangeComponent(3);
                    managerEntity.AddAbilityCurCooldownComponent(0);
                    managerEntity.AddAbilityCastPointComponent(Vector3.zero);
                    managerEntity.AddAbilityUnitTargetCampComponent(Camp.ENEMY);
                    managerEntity.AddAbilityUnitTypeComponent(UnitTypeEnum.MONSER);
                }
            }
        }

        public void Clear()
        {
        }
    }
}