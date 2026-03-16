using UnityEngine;

namespace GamePlay.Runtime
{
    public static class ConstLayer
    {
        public static readonly int Collision = 1 << LayerMask.NameToLayer("Collision");

        public static readonly int CollisionAndOperatedDetectionLayer = 1 << LayerMask.NameToLayer("CollisionAndOperatedDetection");

        public static readonly int OperatedDetection = 1 << LayerMask.NameToLayer("OperatedDetection");

        public static readonly int OperatedLayer = OperatedDetection | CollisionAndOperatedDetectionLayer;

        public static readonly int OperatedCollisionLayer = CollisionAndOperatedDetectionLayer | Collision;

        public static readonly int AllOperatedLayer = CollisionAndOperatedDetectionLayer | OperatedDetection | Collision;
    }
}