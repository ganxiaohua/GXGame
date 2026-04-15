using System.Diagnostics;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class KCCCapability
    {
        private RaycastHit[] raycastHit = new RaycastHit[4];
        private Collider[] overlapCache = new Collider[16];
        private const float BufferAngleShove = 120.0f;
        private const float MaxAngleShoveDegrees = 180.0f - BufferAngleShove;

        // 旋转参数配置
        // public float RotationSpeedMultiplier = 2.0f; // 基础旋转速度系数
        // public float AngleSpeedMultiplier = 1.5f;    // 角速度计算系数
        // public float AirborneRotationPenalty = 0.5f; // 空中旋转减速系数（0.5表示减半）

        // public Vector3 GetDesiredMovement(Vector3 inputMovement)
        // {
        //     Vector3 planeNormal = -Owner.GetGravityAccelerationComp().Value;
        //     var pos = Owner.GetView().Value.Position;
        //     // float dot = Vector3.Dot(Vector3.Project(planeNormal, Vector3.up), Vector3.up);
        //     // bool facingUp = dot > 0 || Mathf.Approximately(dot, 0);
        //     //
        //     // if (previousFacingUp != facingUp)
        //     // {
        //     //     float angle = Vector3.Angle(planeNormal, previousFacingUp ? Vector3.up : Vector3.down);
        //     //     if (Mathf.Abs(angle % 90) <= 80 && !Mathf.Approximately(angle, 180))
        //     //     {
        //     //         facingUp = previousFacingUp;
        //     //     }
        //     // }
        //     //
        //     // previousFacingUp = facingUp;
        //     // Vector3 worldUp = facingUp ? Vector3.up : Vector3.down;
        //
        //     // if (Mathf.Approximately(dot, -1))
        //     // {
        //     //     yaw *= -1;
        //     // }
        //
        //     var planeRotation = Quaternion.FromToRotation(Vector3.up, planeNormal);
        //     // var playerRotation = Quaternion.AngleAxis(0, worldUp);
        //     Vector3 movementForward = planeRotation * Vector3.forward;
        //
        //     Debug.DrawRay(pos, planeNormal, Color.green, 0);
        //     Debug.DrawRay(pos, movementForward, Color.blue, 0);
        //
        //     var movementRotation = Quaternion.LookRotation(movementForward, planeNormal);
        //     Vector3 axisMovement = movementRotation * inputMovement;
        //     return axisMovement;
        // }

        private void CollisionMovement(float deltaTime)
        {
            var view = Owner.GetView().GetData();
            var pos = view.Position;
            // Vector2 oldPos = pos;
            var rot = view.Rotation;
            var gravityVDir = Owner.GetGravityDirVectorComp().Value;
            FollowGround(ref pos, ref rot);
            pos += PushOutOverlapping(pos, rot, 8 * deltaTime, collisionMsg.skinWidth / 2);
            var moveSpeed = Owner.GetMoveSpeedComp().Value;
            var dir = Owner.HasComponent(ComponentsID<GamePlay.Runtime.StopMoveDirection>.TID) ? Vector3.zero : Owner.GetMoveDirectionComp().Value;
            // dir = GetDesiredMovement(dir);
            dir.y = 0;
            dir = (groundMsg.OnGround && groundMsg.Angle <= collisionMsg.maxWalkingAngle) ? Vector3.ProjectOnPlane(dir, groundMsg.RaycastHit.normal) : dir;
            dir = dir.normalized * (moveSpeed * deltaTime);
            if (Owner.HasComponent<MoveDirectionExPowerComp>())
            {
                var exPowerComp = Owner.GetMoveDirectionExPowerComp();
                dir += exPowerComp.Value.Strength * deltaTime;
            }

            // pos = PosOffset(pos, dir);
            pos = PosOffset(pos, dir + gravityVDir * deltaTime);
            if (groundMsg.OnGround && gravityVDir.y <= 0)
                pos = SnapPlayerDown(pos);
            rot = CalculateWorldRotate(rot, deltaTime);
            view.Position = pos;
            view.Rotation = rot;
            UpdateMovingGround(pos, rot);
            // Alert(pos, oldPos);
        }

        private Quaternion CalculateWorldRotate(Quaternion rot, float deltaTime)
        {
            var dir = Owner.GetTurnDirectionComp().Value;
            dir.y = 0;
            if (dir == Vector3.zero)
                return rot;
            float speed = Owner.GetTurnDirectionSpeedComp().Value;
            if (speed == 0)
                return rot;
            if (!groundMsg.OnGround)
                speed /= 2;
            Vector3 nowDir = rot * Vector3.forward;
            float angle = speed * deltaTime;
            var curDir = Vector3.RotateTowards(nowDir, dir, Mathf.Deg2Rad * angle, 0);
            var drot = Quaternion.LookRotation(curDir);
            return drot;
        }


        private Vector3 PushOutOverlapping(Vector3 position, Quaternion rotation, float maxDistance, float skinWidth = 0.0f)
        {
            Vector3 pushed = Vector3.zero;
            var view = Owner.GetView().GetData();
            if (!Owner.HasComponent<ColliderLogicComp>())
                return pushed;
            var colliderComponent = Owner.GetColliderLogicComp();
            var logicData = colliderComponent.GetLogicData();
            int count = 0;
            if (logicData.Type == LogicData.ColliderEnum.CapsuleCollider)
            {
                count = CollisionDetection.OverlapCapsuleNonAlloc(view.gameObject, overlapCache,
                        logicData, position,
                        rotation,
                        collisionMsg.MaskLayer, QueryTriggerInteraction.Collide, skinWidth);
            }
            else if (logicData.Type == LogicData.ColliderEnum.BoxCollider)
            {
                count = CollisionDetection.OverlapBoxNonAlloc(view.gameObject, overlapCache, position + logicData.Center, rotation,
                        logicData.Size, collisionMsg.MaskLayer, skinWidth, QueryTriggerInteraction.Collide);
            }

            for (int i = 0; i < count; i++)
            {
                if (overlapCache[i].gameObject == view.gameObject)
                    continue;
                var overlap = overlapCache[i];
                Physics.ComputePenetration(((LogicView) view).Collider
                        , position, rotation,
                        overlap, overlap.transform.position, overlap.transform.rotation,
                        out Vector3 direction, out float distance
                );
                Vector3 push = direction.normalized * (distance);
                pushed += push;
            }

            return Vector3.ClampMagnitude(pushed, maxDistance);
        }


        private Vector3 PosOffset(Vector3 position, Vector3 movement)
        {
            var view = Owner.GetView().GetData();
            var rotation = view.Rotation;
            Vector3 remaining = movement;
            int bounces = 0;
            //弹跳次数小于最大弹跳次数  &&  移动位置比最小可移动位置大.
            while (bounces < 5 && remaining.magnitude >= collisionMsg.epsilon)
            {
                float distance = remaining.magnitude;
                if (!CastSelf(position, rotation, remaining.normalized, distance, out RaycastHit hit, collisionMsg.MaskLayer, collisionMsg.skinWidth))
                {
                    position += remaining;
                    break;
                }

                if (Vector3.Dot(remaining, movement) < 0)
                {
                    break;
                }

                float fraction = hit.distance / distance;
                Vector3 deltaBounce = remaining * fraction;
                deltaBounce = deltaBounce.normalized * Mathf.Max(0, deltaBounce.magnitude - collisionMsg.epsilon);
                //这里是计算刚好碰到撞击点的距离
                position += deltaBounce;
                //撞击点朝着移动方向延伸出去的一段距离.
                remaining *= (1 - Mathf.Max(0, deltaBounce.magnitude / distance));
                Vector3 planeNormal = hit.normal;
                //上楼梯计算
                bool perpendicularBounce = CheckPerpendicularBounce(hit, remaining);
                Vector3 snappedMomentum = remaining;
                Vector3 snappedPosition = position;
                if (groundMsg.OnGround && perpendicularBounce && AttemptSnapUp(hit, ref snappedMomentum, ref snappedPosition, rotation))
                {
                    if (snappedPosition.magnitude >= collisionMsg.epsilon)
                    {
                        position = snappedPosition;
                    }

                    continue;
                }

                //计算出碰撞曲面与操作方向的夹角
                float angleBetween = Vector3.Angle(hit.normal, remaining);
                float normalizedAngle = Mathf.Max(angleBetween - BufferAngleShove, 0) / MaxAngleShoveDegrees;
                remaining *= Mathf.Pow(Mathf.Abs(1 - normalizedAngle), collisionMsg.anglePower);
                Vector3 projected = Vector3.ProjectOnPlane(remaining, planeNormal).normalized * remaining.magnitude;

                if (projected.magnitude + collisionMsg.epsilon < remaining.magnitude)
                {
                    remaining = Vector3.ProjectOnPlane(remaining, Vector3.up).normalized * remaining.magnitude;
                }
                else
                {
                    remaining = projected;
                }

                // Owner.GetGravityAccelerationComp().Value = -hit.normal * ConstData.DefGravity;
                bounces++;
            }

            return position;
        }

        /// <summary>
        ///  上下楼梯贴地
        /// </summary>
        /// <param name="position"></param>
        /// <returns></returns>
        private Vector3 SnapPlayerDown(Vector3 position)
        {
            var view = Owner.GetView().GetData();
            var rotation = view.Rotation;
            bool hit = CastSelf(
                    position,
                    rotation,
                    Vector3.down,
                    collisionMsg.stepUpDepth,
                    out RaycastHit groundHit,
                    collisionMsg.MaskLayer,
                    collisionMsg.skinWidth);
            if (hit && groundHit.distance > (collisionMsg.groundDist + collisionMsg.skinWidth))
            {
                position += Vector3.down * groundHit.distance + new Vector3(0, collisionMsg.groundDist - collisionMsg.epsilon, 0);
            }

            return position;
        }

        // [Conditional("UNITY_EDITOR")]
        // private void Alert(Vector2 pos, Vector2 oldPos)
        // {
        //     // var comp = Owner.GetColliderComp();
        //     // var assetPathComp = Owner.GetAssetPathComp().Value;
        //     // if (comp == null)
        //     //     return;
        //     // switch (comp.Value)
        //     // {
        //     //     case CapsuleCollider capsuleCollider:
        //     //         Assert.IsFalse(pos.x - oldPos.x > capsuleCollider.radius, $"{assetPathComp} 位移过大，有穿墙风险！");
        //     //         Assert.IsFalse(pos.y - oldPos.y > capsuleCollider.height, $"{assetPathComp} 位移过大，有穿墙风险！");
        //     //         break;
        //     //     case BoxCollider boxCollider:
        //     //         Assert.IsFalse(pos.x - oldPos.x > boxCollider.size.x / 2, $"{assetPathComp} 位移过大，有穿墙风险！");
        //     //         Assert.IsFalse(pos.y - oldPos.y > boxCollider.size.y / 2, $"{assetPathComp} 位移过大，有穿墙风险！");
        //     //         break;
        //     // }
        // }
    }
}