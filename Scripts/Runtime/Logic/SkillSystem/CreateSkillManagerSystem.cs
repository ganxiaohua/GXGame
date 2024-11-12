using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class CreateSkillManagerSystem : IStartSystem<World>
    {
        private Matcher matcher;
        public void Start(World world)
        {
            Matcher matcher = Matcher.SetAll(Components.SkillGroupComponent);
            Group group = world.GetGroup(matcher);
            foreach (var entitie in group.EntitiesMap)
            {
                foreach (var id in entitie.GetSkillGroupComponent().SkillIds)
                {
                    SkillManagerEntity managerEntity = world.AddChild<SkillManagerEntity>();
                    managerEntity.AddSkillIDComponent(id);
                    managerEntity.AddSkillManagerStateComponent(SkillManagerState.None);
                    managerEntity.AddSkillAbilityBehaviorComponent(AbilityBehavior.BEHAVIOR_DIRECTIONAL);
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

        public void Dispose()
        {
        }
    }
}