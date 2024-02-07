using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class SpellStartSystem : IStartSystem<Context>, IUpdateSystem
    {
        private Group m_Group;
        private Context m_Context;

        public void Start(Context entity)
        {
            m_Context = entity;
            Matcher matcher = Matcher.SetAll(Components.SkillIDComponent);
            m_Group = m_Context.GetGroup(matcher);
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var item in m_Group)
            {
                SkillManagerEntity skillentity = (SkillManagerEntity) item;
                if (Input.GetKeyDown(skillentity.GetOnSpellStartComponent().KeyCode))
                {
                    SkillEntity Entity = m_Context.AddChild<SkillEntity>();
                    Entity.AddSkillEffectTargetComponent(new SkillTargetEnum[] {SkillTargetEnum.CASTER});
                    Entity.AddSkillAbilityBehaviorComponent(AbilityBehavior.BEHAVIOR_DIRECTIONAL);
                    Entity.AddSkillComponent();
                    Entity.AddWorldPos(skillentity.GetSkillOwnerComponent().Owner.GetWorldPos().Pos);
                    Entity.AddMoveDirection(item.GetSkillOwnerComponent().Owner.GetDirection().Dir);
                    Entity.AddMoveSpeed(15);
                    Entity.AddSkillOwnerComponent(item.GetSkillOwnerComponent().Owner);
                    Entity.AddSkillCollisionShapeComponent(CollisionShape.Sphere);
                    Entity.AddSkillCollisionRadiusComponent(1);
                    Entity.AddAssetPath("Skill1Effect");
                    Entity.AddViewType(typeof(GameObjectView));
                }
            }
        }

        public void Clear()
        {
        }
    }
}