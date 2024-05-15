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
    struct WorldDirJob : IJobParallelFor
    {
        public NativeArray<float3> MoveDir;
        public NativeArray<float3> NowDir;
        public NativeArray<float> DirSpeed;

        [BurstCompile]
        public void Execute(int index)
        {
            NowDir[index] = Vector3.RotateTowards(NowDir[index], MoveDir[index], Mathf.Deg2Rad * DirSpeed[index], 0);
        }
    }

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
            Job(entities);
        }

        private void Common(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dir = entity.GetMoveDirection().Dir;
                if (dir != Vector3.zero)
                {
                    float speed = entity.GetDirectionSpeed().DirSpeed;
                    Vector3 nowDir = entity.GetDirection().Dir;
                    float angle = speed * Time.deltaTime;
                    Vector3 curDir = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
                    entity.SetDirection(curDir);
                    entity.SetWorldRotate(Quaternion.LookRotation(curDir));
                }
            }
        }

        private void Job(List<ECSEntity> entities)
        {
            NativeArray<float3> movedir = new NativeArray<float3>(entities.Count, Allocator.TempJob);
            NativeArray<float> speed = new NativeArray<float>(entities.Count, Allocator.TempJob);
            NativeArray<float3> curdir = new NativeArray<float3>(entities.Count, Allocator.TempJob);
            int index = 0;
            foreach (var entity in entities)
            {
                movedir[index] = entity.GetMoveDirection().Dir;
                speed[index] = entity.GetDirectionSpeed().DirSpeed * Time.deltaTime;
                curdir[index] = entity.GetDirection().Dir;
                index++;
            }

            WorldDirJob job = new WorldDirJob()
            {
                MoveDir = movedir,
                NowDir = curdir,
                DirSpeed = speed
            };
            var jobHandle = job.Schedule(entities.Count, 4);
            jobHandle.Complete();

            index = 0;
            foreach (var entity in entities)
            {
                entity.SetDirection(job.NowDir[index]);
                entity.SetWorldRotate(Quaternion.LookRotation(job.NowDir[index]));
                index++;
            }

            movedir.Dispose();
            speed.Dispose();
            curdir.Dispose();
        }

        public override void Clear()
        {
        }
    }
}