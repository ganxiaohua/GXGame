using GameFrame.Runtime;
using GamePlay.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class KCCCapability
    {
        private bool CastSelf(Vector3 pos, Quaternion rot, Vector3 dir, float dist, out RaycastHit hit, int layerMask, float skinWidth = 0.01f)
        {
            if (!Owner.HasComponent<ColliderLogicComp>())
            {
                Debug.LogError("错误的分支");
                hit = default;
                return false;
            }

            var view = Owner.GetView().GetData();
            var colliderComp = Owner.GetColliderLogicComp();
            var collider = colliderComp.GetLogicData();
            switch (collider.Type)
            {
                case LogicData.ColliderEnum.CapsuleCollider:
                    return CollisionDetection.CapsuleCastNonAlloc(view.gameObject, raycastHit,
                            collider, pos, rot, dir, dist,
                            out hit,
                            layerMask,
                            skinWidth);
                case LogicData.ColliderEnum.BoxCollider:
                    return CollisionDetection.BoxCastNonAlloc(view.gameObject, raycastHit, pos + collider.Center, rot, dir, dist,
                            collider.Size,
                            out hit, layerMask, skinWidth);
            }

            Debug.LogError("错误的分支");
            hit = default;
            return false;
        }


        private bool CheckPerpendicularBounce(
                RaycastHit hit,
                Vector3 momentum)
        {
            bool hitStep = CollisionDetection.Raycast(
                    hit.point - Vector3.up * collisionMsg.epsilon + hit.normal * collisionMsg.epsilon,
                    momentum.normalized,
                    momentum.magnitude,
                    out RaycastHit stepHit,
                    collisionMsg.MaskLayer,
                    QueryTriggerInteraction.Ignore);
            return hitStep &&
                   Vector3.Dot(stepHit.normal, Vector3.up) <= collisionMsg.epsilon;
        }

        Vector3 GetBottom(Vector3 position, Quaternion rotation)
        {
            if (!Owner.HasComponent<ColliderLogicComp>())
            {
                return position;
            }

            var colliderComp = Owner.GetColliderLogicComp();
            var logicData = colliderComp.GetLogicData();

            switch (logicData.Type)
            {
                case LogicData.ColliderEnum.CapsuleCollider:
                {
                    var ccc = CollisionDetection.CalculateCapsuleCollider(logicData, position, rotation, 0);
                    return ccc.bottom + ccc.radius * (rotation * -Vector3.up);
                }
                case LogicData.ColliderEnum.BoxCollider:
                    return position - new Vector3(0, logicData.Size.y / 2, 0);
                default:
                    Debug.LogError("错误的分支");
                    return position;
            }
        }

        private bool AttemptSnapUp(
                float distanceToSnap,
                ref Vector3 momentum,
                ref Vector3 position,
                Quaternion rotation)
        {
            Vector3 snapPos = position + distanceToSnap * Vector3.up;
            bool didSnapHit = CastSelf(
                    snapPos,
                    rotation,
                    momentum.normalized,
                    momentum.magnitude,
                    out RaycastHit snapHit,
                    collisionMsg.MaskLayer,
                    collisionMsg.skinWidth);
            if (!didSnapHit)
            {
                float distanceMove = Mathf.Min(momentum.magnitude, distanceToSnap);
                position += distanceMove * Vector3.up;
                return true;
            }

            return false;
        }


        private bool AttemptSnapUp(
                RaycastHit hit,
                ref Vector3 momentum,
                ref Vector3 position,
                Quaternion rotation)
        {
            Vector3 bottom = GetBottom(position, rotation);
            Vector3 footVector = Vector3.Project(hit.point, Vector3.up) - Vector3.Project(bottom, Vector3.up);
            bool isAbove = Vector3.Dot(footVector, Vector3.up) > 0;
            float distanceToFeet = footVector.magnitude * (isAbove ? 1 : -1);
            bool snappedUp = false;
            if (distanceToFeet < collisionMsg.stepUpDepth)
            {
                snappedUp = AttemptSnapUp(
                        distanceToFeet,
                        ref momentum,
                        ref position,
                        rotation);
                //操作指令方向爬不上就设置攀爬高度在尝试一下
                if (!snappedUp)
                {
                    snappedUp = AttemptSnapUp(
                            collisionMsg.stepUpDepth,
                            ref momentum,
                            ref position,
                            rotation);
                }
            }

            return snappedUp;
        }
    }
}