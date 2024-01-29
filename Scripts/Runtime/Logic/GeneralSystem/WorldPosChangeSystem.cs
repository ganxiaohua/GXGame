using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class WorldPosChangeSystem : ReactiveSystem
    {
        private Group Group;

        public override void Start(Context entity)
        {
            base.Start(entity);
            Matcher matcher = Matcher.SetAllOfIndices(Components.WorldPos, Components.MoveSpeed);
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
                    float speed = entity.GetMoveSpeed().Speed;
                    Vector3 inputDir = dir * (speed * Time.deltaTime);
                    Vector3 pos = entity.GetWorldPos().Pos + inputDir;
                    entity.SetWorldPos(pos);
                }
            }
        }

        public override void Clear()
        {
        }
    }
}