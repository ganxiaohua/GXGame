using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime.MapData
{
    [Serializable]
    public class ChunkHouseData : BaseRST
    {
        [ReadOnly]
        public int HouseId;
    }

    [Serializable]
    public class HouseData : ScriptableObject
    {
        [ReadOnly]
        public int Id;

        [ReadOnly]
        public List<Unit> Units;

        [ReadOnly]
        public List<PortalUnit> ProtalUnit;
    }
}