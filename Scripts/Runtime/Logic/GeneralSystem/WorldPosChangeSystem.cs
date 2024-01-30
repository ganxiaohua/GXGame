using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class WorldPosChangeSystem : ReactiveSystem
    {
        protected override Collector GetTrigger(Context context) => Collector.CreateCollector(context, Components.MoveDirection);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent(Components.WorldPos) && entity.HasComponent(Components.MoveSpeed);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
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