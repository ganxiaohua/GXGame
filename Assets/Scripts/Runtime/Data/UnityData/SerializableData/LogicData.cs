using System;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime
{
    [System.Serializable]
    public class LogicData : ScriptableObject
    {
        public enum ColliderEnum
        {
            None,
            BoxCollider,
            SphereCollider,
            CapsuleCollider,
        }

        [ReadOnly]
        public int Layer;

        [ReadOnly]
        public ColliderEnum Type;

        [ReadOnly]
        public bool IsTrigger;

        public Vector3 Pos;
        public Quaternion Rot;
        public Vector3 Scale;

        // 通用属性
        [ReadOnly]
        public Vector3 Center;

        // BoxCollider 特有
        [ReadOnly]
        public Vector3 Size;

        // SphereCollider 特有
        [ReadOnly]
        public float Radius;

        // CapsuleCollider 特有
        [ReadOnly]
        public float Height;

        [ReadOnly]
        public int Direction;
    }
}