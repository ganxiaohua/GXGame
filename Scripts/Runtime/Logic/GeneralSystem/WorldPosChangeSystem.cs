using System.Collections.Generic;
using GameFrame;
using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;

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
        private RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[4];

        protected override Collector GetTrigger(World world) =>
            Collector.CreateCollector(world, Collector.ChangeEventState.AddUpdate, Components.MoveDirection);

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
                var speed = entity.GetMoveSpeed().Speed * World.DeltaTime;
                var pos = entity.GetWorldPos().Pos;
                entity.SetWorldPos(CollisionSlide(pos, dir, speed));
            }
        }

        private void Job(List<ECSEntity> entities)
        {
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
                var entitie = entities[i];
                Vector2 cv = CollisionSlide(entitie.GetWorldPos().Pos, entitie.GetMoveDirection().Dir, entitie.GetMoveSpeed().Speed);
                float3 com = new float3(cv.x, cv.y, 0);
                entitie.SetWorldPos(job.CurPos[i] + com);
            }

            dir.Dispose();
            speed.Dispose();
            curpos.Dispose();
        }

        private Vector2 CollisionSlide(Vector2 pos, Vector2 dir, float speed)
        {
            dir = dir.normalized;
            Vector2 nextPos = pos + dir * speed;
            int count = Physics2D.BoxCast(nextPos, Vector2.one * 0.5f, 0, Vector2.zero, default, raycastHit2Ds);
            if (count == 0) return nextPos;
            RaycastHit2D targetRaycastHit2D = raycastHit2Ds[0];
            Vector2 projection = Vector2.Dot(-dir, targetRaycastHit2D.normal) / dir.sqrMagnitude * targetRaycastHit2D.normal.normalized;
            nextPos = pos + (dir + projection).normalized * speed;
            for (int i = 0; i < 3; i++)
            {
                count = Physics2D.BoxCast(nextPos, Vector2.one * 0.5f, 0, Vector2.zero, default, raycastHit2Ds);
                if (count !=0)
                {
                    targetRaycastHit2D = raycastHit2Ds[0];
                    nextPos += targetRaycastHit2D.normal.normalized * speed;
                }
                else
                {
                    break;
                }   
            }
            return count == 0 ? nextPos : pos;
        }

        public override void Dispose()
        {
        }
    }
}