using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public class CollisionBehaviorSystem : UpdateReactiveSystem
    {
        protected override Collector GetTrigger(World world) =>
                Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.Update, ComponentsID<RaycastHit>.TID);

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Execute(ECSEntity entity)
        {
            // foreach (var entity in entities)
            // {
            var raycastHits = entity.GetRaycastHit();
            foreach (var raycastHit in raycastHits.Value)
            {
                var campValue = raycastHit.transform.GetComponent<CollisionEntity>().Entity.GetCampComponent().Value;
                if (campValue != entity.GetCampComponent().Value)
                {
                    var hp = entity.GetHP().Value - 1;
                    entity.SetHP(hp);
                    if (hp == 0)
                    {
                        entity.AddDestroy();
                    }

                    break;
                }
            }

            entity.GetRaycastHit().Value.Clear();
            // }
        }

        public override void Dispose()
        {
        }
    }
}