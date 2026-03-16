using System;
using System.Collections.Generic;
using GameFrame.Runtime;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime.MapData
{
    [Serializable]
    public class AreaData : ScriptableObject
    {
        [ReadOnly]
        public int Id;

        [ReadOnly] [ShowInInspector]
        public Vector2 StartPoint;

        [ReadOnly] [ShowInInspector]
        public Vector2Int Size;

        [ReadOnly] [ShowInInspector]
        public Vector2Int CellSize;

        [ReadOnly] [ShowInInspector]
        public Vector2Int ChunkSize;

        [ReadOnly] [ShowInInspector]
        public int ChunkCount;

        [ShowInInspector]
        public List<int> CropLand;
    }
}