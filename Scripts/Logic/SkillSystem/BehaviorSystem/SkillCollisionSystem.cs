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
            foreach (SkillEntity item in entities)
            {
                Collider[] collider = Physics.OverlapSphere(item.GetWorldPos().Pos, 1);
                foreach (var colliderItem in collider)
                {
                    Debug.Log(colliderItem.transform.name); 
                }
            }
        }
        
        
        

        public override void Clear()
        {
            
        }
    }
}