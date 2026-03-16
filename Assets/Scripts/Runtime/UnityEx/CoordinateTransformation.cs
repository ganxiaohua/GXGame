using UnityEngine;

namespace GamePlay.Runtime
{
    public static class CoordinateTransformation
    {
        public static Vector3 XZCoordinateCVector3(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }

        public static Vector3 XZCoordinateCVector3(this Vector2Int vector2)
        {
            return new Vector3(vector2.x, 0, vector2.y);
        }

        public static Vector3Int XZCoordinateCVector3Int(this Vector2Int vector2)
        {
            return new Vector3Int(vector2.x, 0, vector2.y);
        }


        public static Vector2 XZCoordinateCVector2(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
    }
}