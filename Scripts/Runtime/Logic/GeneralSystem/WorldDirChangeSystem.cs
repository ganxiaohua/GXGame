using System.Collections.Generic;
using GameFrame;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace GXGame
{
    public class WorldDirChangeSystem : ReactiveSystem
    {
        public override void Start(Context entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) =>
            Collector.CreateCollector(context, Components.MoveDirection, Components.Direction, Components.DirectionSpeed, Components.WorldRotate);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.GetDirectionSpeed() != null && entity.GetDirection() != null && entity.GetMoveDirection() != null && entity.GetWorldRotate() != null;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dir = entity.GetMoveDirection().Dir;
                if (dir != Vector3.zero)
                {
                    float speed = entity.GetDirectionSpeed().Speed;
                    Vector3 nowDir = entity.GetDirection().Dir;

                    float angle = speed*Time.deltaTime;
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