using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 碰到了就修正方向
    /// </summary>
    public partial class CollisionSystem : FixedUpdateReactiveSystem
    {
        private RaycastHit2D[] raycastHit2Ds = new RaycastHit2D[4];

        protected override Collector GetTrigger(World world) =>
            Collector.CreateCollector(world, EcsChangeEventState.ChangeEventState.AddUpdate, Components.MoveDirection);

        protected override bool Filter(ECSEntity entity)
        {
            return entity.HasComponent(Components.CollisionBox) &&
                   entity.HasComponent(Components.MoveDirection) && entity.HasComponent(Components.MoveSpeed) && entity.HasComponent(Components.WorldPos);
        }

        protected override void Execute(List<ECSEntity> entities)
        {
            foreach (var entity in entities)
            {
                var dir = entity.GetMoveDirection().Value;
                if (dir == Vector3.zero)
                    continue;
                var distance = entity.GetMoveSpeed().Value * Time.deltaTime * World.Multiple;
                var pos = entity.GetWorldPos().Value;
                dir = dir.normalized;
                dir = GetCollisionDir(pos, dir, distance, entity);
                entity.SetMoveDirection(dir);
                pos += dir * distance;
                var collisionBox = entity.GetCollisionBox();
                collisionBox.Value.position = pos;
                entity.SetCollisionBox(collisionBox.Value);
            }
        }

        private Vector2 GetCollisionDir(Vector2 pos, Vector2 dir, float distance, ECSEntity entity)
        {
            var box = entity.GetCollisionBox();
            var count = BoxCast(pos, dir, distance, box);
            if (count == 0) return dir;
            var collisonProority = CollisionPriority(entity);
            RaycastHit2D targetRaycastHit2D = collisonProority.hit2D;
            if (collisonProority.priority == 0)
            {
                return dir;
            }
            else if (collisonProority.priority == 2)
            {
                var hit = entity.GetRaycastHit();
                if (hit == null)
                {
                    entity.AddRaycastHit(new List<RaycastHit2D>());
                    hit = entity.GetRaycastHit();
                }
                hit.Value.Add(targetRaycastHit2D);
                entity.SetRaycastHit(hit.Value);
                return Vector2.zero;
            }

            if ((entity.GetCollisionGroundType().Type == CollisionGroundType.Slide))
            {
                var rayNormal = targetRaycastHit2D.normal.normalized;
                Vector2 projection = Vector2.Dot(-dir, rayNormal) / rayNormal.sqrMagnitude * rayNormal;
                dir = (dir + projection).normalized;
            }
            else if (entity.GetCollisionGroundType().Type == CollisionGroundType.Reflect)
            {
                dir = Vector2.Reflect(dir, targetRaycastHit2D.normal).normalized;
            }
            else if (entity.GetCollisionGroundType().Type == CollisionGroundType.Bomb)
            {
                dir = -dir;
            }
            return dir;
        }

        private int BoxCast(Vector2 pos, Vector2 dir, float distance, CollisionBox box)
        {
            int count = Physics2D.BoxCastNonAlloc(pos, Vector2.one * 0.5f, 0, dir, raycastHit2Ds, distance);
            for (int i = 0; i < count; i++)
            {
                if (raycastHit2Ds[i].transform == box.Value.transform)
                {
                    raycastHit2Ds[i] = raycastHit2Ds[count - 1];
                    raycastHit2Ds[count - 1] = default;
                    count--;
                    break;
                }
            }

            return count;
        }

        /// <summary>
        /// 碰触优先级,碰到其他东西的优先级 并且过滤掉同阵营的
        /// </summary>
        /// <returns></returns>
        private (int priority, RaycastHit2D hit2D) CollisionPriority(ECSEntity owner)
        {
            int priority = 0;
            RaycastHit2D hit2D = default;
            var camp = owner.GetCampComponent().Value;
            for (int i = 0; i < raycastHit2Ds.Length; i++)
            {
                if (raycastHit2Ds[i] == default)
                    continue;

                if (priority < 2 && raycastHit2Ds[i].transform.gameObject.layer == LayerMask.NameToLayer($"Object"))
                {
                    //过滤掉同阵营
                    var rayCamp = raycastHit2Ds[i].transform.GetComponent<CollisionEntity>().Entity.GetCampComponent();
                    if (rayCamp != null && rayCamp.Value == camp)
                    {
                        continue;
                    }

                    priority = 2;
                    hit2D = raycastHit2Ds[i];
                }
                else if (priority < 1 && raycastHit2Ds[i].transform.gameObject.layer == LayerMask.NameToLayer($"Ground"))
                {
                    priority = 1;
                    hit2D = raycastHit2Ds[i];
                }
            }

            return (priority, hit2D);
        }


        public override void Dispose()
        {
        }
    }
}