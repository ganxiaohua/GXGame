using System.Collections.Generic;
using Pathfinding;
using UnityEngine;

namespace GameFrame.Runtime
{
    public class SeekerData
    {
        public enum StateEnum
        {
            None,
            Find,
            FindOver,
        }

        public Seeker Seeker;
        public Path Path;
        public List<Vector3> VectorPath;
        public int CurIndex;
        public StateEnum State;
    }

    public struct SeekerComp : EffComponent
    {
        private int valueIndex;

        public void Init(SeekerData data)
        {
            valueIndex = ObjectDatas<SeekerData>.Instance.AddData(data);
        }

        public SeekerData GetData()
        {
            return ObjectDatas<SeekerData>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<SeekerData>.Instance.RemoveData(valueIndex);
        }
    }
}