using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class KCCCapability
    {
        private readonly float jumpCd = 0.5f;
        private float curJumpCd = 0;

        private void Jump(float deltaTime)
        {
            var dir = Owner.GetMoveDirectionComp().Value;
            var ga = Owner.GetGravityAccelerationComp().Value;
            var jumpSpeed = Owner.GetJumpSpeedComp().Value;
            bool fg = IsFallingOrglide();
            var velocity = Owner.GetGravityDirVectorComp().Value;
            if (fg)
            {
                velocity += ga * deltaTime;
            }
            else
            {
                velocity = Vector3.zero;
                curJumpCd -= deltaTime;
                curJumpCd = Mathf.Max(0, curJumpCd);
            }

            bool canJump = (groundMsg.OnGround) && groundMsg.Angle <= collisionMsg.maxJumpAngle && !fg;
            if (canJump && dir.y > 0 && curJumpCd == 0)
            {
                velocity = Vector3.Lerp(-ga.normalized, (groundMsg.RaycastHit.normal).normalized, collisionMsg.jumpAngleWeightFactor).normalized * jumpSpeed;
                curJumpCd = jumpCd;
            }
            dir.y = 0;
            Owner.SetMoveDirectionComp(dir);
            Owner.SetGravityDirVectorComp(velocity);
        }


        private bool IsFallingOrglide()
        {
            return !(groundMsg.OnGround && groundMsg.Angle <= collisionMsg.maxWalkingAngle);
        }
    }
}