using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public struct ColliderLogicComp : EffComponent
    {
        private int ValueIndex;

        public void Init(LogicData data)
        {
            ValueIndex = ObjectDatas<LogicData>.Instance.AddData(data);
        }

        public LogicData GetLogicData()
        {
            return ObjectDatas<LogicData>.Instance.GetData(ValueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<LogicData>.Instance.RemoveData(ValueIndex);
        }
    }

    /// <summary>
    /// 碰撞范围
    /// </summary>
    public struct VirtualCollision
    {
        public Vector3 Pos;
        public Quaternion Rot;
        public Vector3 Size;
        public int Layer;
    }

    public struct OperatedDetectionData
    {
        public int OperatorCount;
        public OperatedType OperatedType;

        /// <summary>
        /// 携带一个手中物品的ID，没有就算。
        /// </summary>
        public int Itemid;
    }

    public struct OperatedDetectionComp : EffComponent
    {
        public OperatedDetectionData Value;

        public void Dispose()
        {
        }
    }


    public struct CollisionDetectionDataComp : EffComponent
    {
        public VirtualCollision Value;

        public void Dispose()
        {
        }
    }
}