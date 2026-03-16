using System;
using UnityEngine;

namespace GamePlay.Runtime.MapData
{
    [Serializable]
    public class BaseRST
    {
        /// <summary>
        /// 唯一编号,整个场景中所有东西都是公用一套0 - 正无穷
        /// </summary>
        [Sirenix.OdinInspector.ReadOnly]
        public int Index;

        [Sirenix.OdinInspector.ReadOnly]
        public Vector3 lPos;

        [Sirenix.OdinInspector.ReadOnly]
        public Vector3 lScale;

        [Sirenix.OdinInspector.ReadOnly]
        public Quaternion lRot;
    }
}