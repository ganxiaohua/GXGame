using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class WorldDirChangeSystem : ReactiveSystem
    {
        public override void Start(Context entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) => Collector.CreateCollector(context, Components.MoveDirection);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent((Components.WorldRotate)) && entity.HasComponent(Components.Direction) && entity.HasComponent(Components.DirectionSpeed);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dir = entity.GetMoveDirection().Dir;
                if (dir != Vector3.zero)
                {
                    float speed = entity.GetDirectionSpeed().DirSpeed;
                    Vector3 nowDir = entity.GetDirection().Dir;
            
                    float angle = speed * Time.deltaTime;
                    Vector3 curNow = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
            
                    entity.SetDirection(curNow);
                    entity.SetWorldRotate(Quaternion.LookRotation(curNow));
                }
            }
        }

        public override void Clear()
        {
        }
    }
}