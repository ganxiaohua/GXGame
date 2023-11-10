using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class SpellStartSystem : ReactiveSystem
    {
        protected override Collector GetTrigger(Context context)
        {
            return Collector.CreateCollector(context, Components.SkillIDComponent, Components.OnSpellStartComponent);
        }

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent(Components.OnSpellStartComponent);
        }

        protected override void Update(List<ECSEntity> entities)
        {
            //更具配表或者其他的数据来源给技能管理器添加上,比如播放声音,加上特效
            foreach (var item in entities)
            {
                SkillManagerEntity skillentity = (SkillManagerEntity) item;
                if (Input.GetKeyDown(skillentity.GetOnSpellStartComponent().KeyCode))
                {
                    SkillEntity Entity = Context.AddChild<SkillEntity>();
                    Entity.AddSkillEffectComponent(new string[] {"Skill1Effect"});
                    Entity.AddSkillEffectTargetComponent(new SkillTargetEnum[] {SkillTargetEnum.CASTER});
                    Entity.AddSkillAbilityBehaviorComponent(AbilityBehavior.BEHAVIOR_DIRECTIONAL);
                    Entity.AddSkillComponent();
                    Entity.AddWorldPos(skillentity.GetSkillOwnerComponent().Owner.GetWorldPos().Pos);
                    Entity.AddMoveDirection(item.GetSkillOwnerComponent().Owner.GetDirection().Dir);
                    Entity.AddMoveSpeed(15);
                    Entity.AddSkillOwnerComponent(item.GetSkillOwnerComponent().Owner);
                    Entity.AddSkillCollisionShapeComponent(CollisionShape.Sphere);
                    Entity.AddSkillCollisionRadiusComponent(1);
                }
            }
        }

        public override void Clear()
        {
        }
    }
}