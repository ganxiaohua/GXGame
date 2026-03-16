using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public struct GroundCollision
    {
        public bool OnGround;
        public float Angle;
        public RaycastHit RaycastHit;
    }

    public struct GroundCollisionComp : EffComponent
    {
        public GroundCollision Value;

        public bool Equatable(GroundCollision groundCollision)
        {
            return Value.OnGround != groundCollision.OnGround;
        }

        public void Dispose()
        {
        }
    }


    public class PreviousGroundMsg
    {
        public Vector3 RelativePos;
        public Quaternion RelativeRotation;
        public Transform PreviousParent;
    }

    public struct PreviousGroundComp : EffComponent
    {
        private int valueIndex;

        public void Init(PreviousGroundMsg data)
        {
            valueIndex = ObjectDatas<PreviousGroundMsg>.Instance.AddData(data);
        }

        public PreviousGroundMsg GetData()
        {
            return ObjectDatas<PreviousGroundMsg>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<PreviousGroundMsg>.Instance.RemoveData(valueIndex);
        }
    }
}