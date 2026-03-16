using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime.MapData
{
    [Serializable]
    public class Unit : BaseRST
    {
        [ReadOnly]
        public int UnitId;
    }

    [Serializable]
    public class PortalUnit : Unit
    {
        public int PortaTagertlID;
        public Vector3 ProtalTargetPoint;
    }
}