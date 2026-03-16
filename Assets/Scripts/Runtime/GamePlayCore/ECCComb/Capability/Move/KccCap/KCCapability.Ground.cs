using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class KCCCapability
    {
        private void CheckGroundedCapsule()
        {
            var view = Owner.GetView().GetData();
            var pos = view.Position;
            var rot = view.Rotation;
            var colliderData = Owner.GetColliderLogicComp().GetLogicData();
            var gravity = Owner.GetGravityAccelerationComp().Value;
            RaycastHit groundHit = default;
            bool onGround = false;
            if (colliderData.Type == LogicData.ColliderEnum.CapsuleCollider)
            {
                onGround = CollisionDetection.CapsuleCastNonAlloc(
                        view.gameObject,
                        raycastHit,
                        colliderData, pos, rot, gravity.normalized, collisionMsg.groundDist, out groundHit,
                        collisionMsg.MaskLayer, collisionMsg.skinWidth);
            }
            else if (colliderData.Type == LogicData.ColliderEnum.BoxCollider)
            {
                onGround = CollisionDetection.BoxCastNonAlloc(view.gameObject, raycastHit, pos, rot, gravity.normalized,
                        collisionMsg.groundDist, colliderData.Size, out groundHit, collisionMsg.MaskLayer, collisionMsg.skinWidth);
            }

            float angle = Vector3.Angle(groundHit.normal, -gravity.normalized);
            groundMsg.OnGround = onGround;
            groundMsg.Angle = angle;
            groundMsg.RaycastHit = groundHit;
            Owner.SetGroundCollisionComp(groundMsg);
        }
    }
}