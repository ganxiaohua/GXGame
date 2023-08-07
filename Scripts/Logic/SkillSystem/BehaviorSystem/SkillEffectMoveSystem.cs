using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class SkillEffectMoveSystem : ReactiveSystem
    {
        protected override Collector GetTrigger(Context context)
        {
            return Collector.CreateCollector(context, Components.SkillComponent,Components.SkillEffectEnitiyComponent);
        }

        protected override bool Filter(ECSEntity entity)
        {
            var enitiy = (SkillEffectEnitiyComponent)entity.GetComponent(Components.SkillEffectEnitiyComponent);
            if (entity.HasComponent(Components.SkillComponent) && enitiy!=null && enitiy.HasEffect == false)
            {
                return true;
            }
            return false;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (SkillEntity item in entities)
            {
                SkillEffectEntity skilleffect = Context.AddChild<SkillEffectEntity>();
                var cubeHero = item.GetSkillOwnerComponent().Owner;
                skilleffect.AddWorldPos(cubeHero.GetWorldPos().Pos);
                skilleffect.AddAssetPath("Skill1Effect");
                skilleffect.AddViewType(typeof(GameObjectView));
                // skilleffect.AddMoveDirection(cubeHero.GetDirection().Dir);
                // skilleffect.AddMoveSpeed(10);
                skilleffect.AddSkillOwnerComponent(item);
                item.SetSkillEffectEnitiyComponent(true);
            }
        }
        
        
        

        public override void Clear()
        {
            
        }
    }
}