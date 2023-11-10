using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class WorldPosChangeSystem : ReactiveSystem
    {
        public override void Start(Context entity)
        {
            base.Start(entity);
        }

        protected override Collector GetTrigger(Context context) =>
            Collector.CreateCollector(context,Components.MoveDirection,Components.WorldPos,Components.MoveSpeed);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.GetMoveSpeed() != null;
        }

        protected override void Update(List<ECSEntity> entities)
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