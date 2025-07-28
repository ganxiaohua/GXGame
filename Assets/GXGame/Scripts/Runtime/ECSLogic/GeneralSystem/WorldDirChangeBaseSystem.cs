using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public class WorldDirChangeBaseSystem : UpdateReactiveSystem
    {
        protected override Collector GetTrigger(World world) =>
                Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddUpdate,
                        ComponentsID<FaceDirection>.TID);

        protected override bool Filter(EffEntity entity)
        {
            return !entity.HasComponent(ComponentsID<CollisionBox>.TID) && entity.HasComponent(ComponentsID<WorldRotate>.TID) &&
                   entity.HasComponent(ComponentsID<FaceDirection>.TID) &&
                   entity.HasComponent(ComponentsID<DirectionSpeed>.TID);
        }

        protected override void Execute(List<EffEntity> entitys)
        {
            foreach (var entity in entitys)
            {
                var dir = entity.GetFaceDirection().Value;
                if (dir != Vector3.zero)
                {
                    float speed = entity.GetDirectionSpeed().Value;
                    Vector3 nowDir = entity.GetWorldRotate().Value * Vector3.forward;
                    float angle = speed * World.DeltaTime;
                    Vector3 curDir = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
                    entity.SetWorldRotate(Quaternion.LookRotation(curDir));
                }
            }
        }

        public override void Dispose()
        {
        }
    }
}