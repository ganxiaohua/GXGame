using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime.MapData
{
    [Serializable]
    public class AreaChunkData : ScriptableObject
    {
        [ReadOnly]
        public int Id;

        [ReadOnly]
        public List<Unit> Units;

        [ReadOnly]
        public List<ChunkHouseData> ChunkHouseList;

        [ReadOnly]
        public List<PortalUnit> ProtalUnit;

        public bool HaveAStarPath;

        public List<int> CroplandData;
    }
}