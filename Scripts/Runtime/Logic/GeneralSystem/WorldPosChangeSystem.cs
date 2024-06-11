using System.Collections.Generic;
using GameFrame;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine.Tilemaps;

namespace GXGame
{
    [BurstCompile]
    struct WorldPosJob : IJobParallelFor
    {
        public NativeArray<float3> Dir;
        public NativeArray<float> Speed;
        public NativeArray<float3> CurPos;

        [BurstCompile]
        public void Execute(int index)
        {
            CurPos[index] += Dir[index] * Speed[index];
        }
    }

    public class WorldPosChangeSystem : ReactiveSystem
    {
        protected override Collector GetTrigger(World world) => Collector.CreateCollector(world, Collector.ChangeEventState.AddUpdate,Components.MoveDirection,Components.MoveSpeed,Components.WorldPos);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent(Components.MoveDirection) &&  entity.HasComponent(Components.MoveSpeed) && entity.HasComponent(Components.WorldPos);
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
                var speed = entity.GetMoveSpeed().Speed * World.DeltaTime;
                var pos = entity.GetWorldPos().Pos;
                entity.SetWorldPos(pos+speed*dir);
            }
        }
        
        private void Job(List<ECSEntity> entities){
            NativeArray<float3> dir = new NativeArray<float3>(entities.Count, Allocator.TempJob);
            NativeArray<float> speed = new NativeArray<float>(entities.Count, Allocator.TempJob);
            NativeArray<float3> curpos = new NativeArray<float3>(entities.Count, Allocator.TempJob);
            int index = 0;
            foreach (var entity in entities)
            {
                dir[index] = entity.GetMoveDirection().Dir;
                speed[index] = entity.GetMoveSpeed().Speed * World.DeltaTime;
                curpos[index] = entity.GetWorldPos().Pos;
                index++;
            }
            
            WorldPosJob job = new WorldPosJob()
            {
                Dir = dir,
                Speed = speed,
                CurPos = curpos
            };
            var jobHandle = job.Schedule(entities.Count, 4);
            jobHandle.Complete();
            
            for (int i = 0; i < entities.Count; i++)
            {
                entities[i].SetWorldPos(job.CurPos[i]);
            }
            
            dir.Dispose();
            speed.Dispose();
            curpos.Dispose();
        }

        public override void Clear()
        {
        }
    }
}