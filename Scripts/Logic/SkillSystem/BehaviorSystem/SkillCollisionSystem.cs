using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class SkillCollisionSystem : ReactiveSystem
    {
        protected override Collector GetTrigger(Context context)
        {
            return Collector.CreateCollector(context, Components.SkillCollisionShapeComponent);
        }

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (SkillEffectEntity item in entities)
            {
                Collider[] collider = Physics.OverlapSphere(item.GetSkillOwnerComponent().Owner.GetWorldPos().Pos, 1);
            }
        }
        
        
        

        public override void Clear()
        {
            
        }
    }
}