using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class SkillEffectMoveSystem : ReactiveSystem
    {
        protected override Collector GetTrigger(Context context)
        {
            return Collector.CreateCollector(context, Components.SkillIsEffectComponent);
        }

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (SkillEffectEntity item in entities)
            {
                item.SetWorldPos(item.GetSkillOwnerComponent().Owner.GetWorldPos().Pos);
            }
        }
        
        
        

        public override void Clear()
        {
            
        }
    }
}