using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class TargetSystem : IInitializeSystem<World>, IUpdateSystem
    {
        private Group group1;

        public void OnInitialize(World world)
        {
            Matcher matcher = Matcher.SetAll(Components.WorldPos, Components.MoveDirection, Components.FaceDirection)
                .Any(Components.TargetEntity, Components.TargetPos);
            group1 = world.GetGroup(matcher);
        }

        public void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            foreach (var entity in group1)
            {
                var targetPos = Vector3.zero;
                var speed = entity.GetMoveSpeed().Speed;
                if(speed == 0)
                    continue;
                if (entity.HasComponent(Components.TargetPos))
                {
                    targetPos = entity.GetTargetPos().Target;
                }
                else if (entity.HasComponent(Components.TargetEntity))
                {
                    targetPos = entity.GetTargetEntity().Target.GetWorldPos().Pos;
                }
            
                var dir = targetPos - entity.GetWorldPos().Pos;
                dir = dir.normalized;
                entity.SetFaceDirection(dir);
                entity.SetMoveDirection(dir);
            }
        }

        public void Dispose()
        {
        }
    }
}