using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class SkillEffectCreateSystem : ReactiveSystem
    {
        
        protected override Collector GetTrigger(Context context)
        {
            return Collector.CreateCollector(context, Components.SkillComponent);
        }

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (SkillEntity item in entities)
            {
                SkillEffectEntity skilleffect = Context.AddChild<SkillEffectEntity>();
                skilleffect.AddWorldPos(item.GetWorldPos().Pos);
                skilleffect.AddAssetPath("Skill1Effect");
                skilleffect.AddViewType(typeof(GameObjectView));
                skilleffect.AddSkillIsEffectComponent();
                skilleffect.AddSkillOwnerComponent(item);
                item.AddSkillEffectEnitiyComponent(skilleffect);
            }
        }
        
        
        

        public override void Clear()
        {
            
        }
    }
}