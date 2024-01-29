using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class WorldDirChangeSystem : ReactiveSystem
    {
        private Group Group;

        public override void Start(Context entity)
        {
            base.Start(entity);

            Matcher matcher = Matcher.SetAllOfIndices(Components.Direction, Components.DirectionSpeed, Components.WorldRotate);
            Group = entity.GetGroup(matcher);
        }

        protected override Collector GetTrigger(Context context) => Collector.CreateCollector(context, Components.MoveDirection);

        protected override bool Filter(ECSEntity entity)
        {
            return true;
        }

        protected override void Update(List<ECSEntity> entities)
        {
            foreach (var entity in Group)
            {
                var dir = entity.GetMoveDirection().Dir;
                if (dir != Vector3.zero)
                {
                    float speed = entity.GetDirectionSpeed().Speed;
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