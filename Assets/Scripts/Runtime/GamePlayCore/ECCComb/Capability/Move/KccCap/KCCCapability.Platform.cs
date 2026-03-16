using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class KCCCapability
    {
        private void UpdateMovingGround(Vector3 position, Quaternion rotation)
        {
            if (!groundMsg.OnGround)
            {
                previousGround.RelativePos = Vector3.zero;
                previousGround.RelativeRotation = Quaternion.identity;
                previousGround.PreviousParent = null;
                return;
            }

            var parent = groundMsg.RaycastHit.transform;
            var parentInverse = Quaternion.Inverse(parent.rotation);
            //得到我在父物体中的本地旋转
            previousGround.RelativeRotation = rotation * parentInverse;
            //得到我在父物体中的本地坐标.
            previousGround.RelativePos = position - parent.position;
            previousGround.RelativePos = parentInverse * previousGround.RelativePos;
            previousGround.PreviousParent = parent;
        }


        private Quaternion DeltaRotation(Quaternion a)
        {
            if (!groundMsg.OnGround || !previousGround.PreviousParent)
            {
                return a;
            }

            var rotation = previousGround.RelativeRotation * previousGround.PreviousParent.rotation;
            return rotation;
        }

        private Vector3 DeltaPosition(Vector3 position)
        {
            var value = previousGround;
            if (!groundMsg.OnGround || value.PreviousParent == null)
            {
                return Vector3.zero;
            }

            return (value.PreviousParent.position + value.PreviousParent.rotation * value.RelativePos) - position;
        }

        private void FollowGround(ref Vector3 position, ref Quaternion rotation)
        {
            position += DeltaPosition(position);
            rotation = DeltaRotation(rotation);
        }
    }
}