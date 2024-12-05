using System.Collections.Generic;
using GameFrame;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

namespace GXGame
{
    public class WorldPosChangeSystem : ReactiveSystem
    {
        private RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[4];

        protected override Collector GetTrigger(World world) =>
            Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddUpdate, Components.MoveDirection);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent(Components.MoveDirection) && entity.HasComponent(Components.MoveSpeed) && entity.HasComponent(Components.WorldPos);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            Common(entities);
        }

        private void Common(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dir = entity.GetMoveDirection().Dir;
                var distance = entity.GetMoveSpeed().Speed * World.DeltaTime;
                var pos = entity.GetWorldPos().Pos;
                pos += (dir * distance);
                entity.SetWorldPos(pos);
            }
        }

        public override void Dispose()
        {
        }
    }
}