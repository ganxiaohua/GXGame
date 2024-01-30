using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class SkillCollisionSystem : ReactiveSystem
    {
        private int m_Terrain = LayerMask.NameToLayer(nameof(Terrain));
        Collider[] m_Collisins = new Collider[8];

        protected override Collector GetTrigger(Context context)
        {
            return Collector.CreateCollector(context, Components.SkillCollisionShapeComponent,Components.WorldPos);
        }

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent((Components.SkillCollisionShapeComponent)) && entity.HasComponent(Components.WorldPos);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var ecsEntity in entities)
            {
                var item = (SkillEntity) ecsEntity;
                var size = Physics.OverlapSphereNonAlloc(item.GetWorldPos().Pos, item.GetSkillCollisionRadiusComponent().Radius, m_Collisins);
                for (int i = 0; i < size; i++)
                {
                    if (m_Collisins[i].gameObject.layer == m_Terrain)
                    {
                        item.AddDestroy();
                        return;
                    }
                }
            }
        }


        public override void Clear()
        {
        }
    }
}