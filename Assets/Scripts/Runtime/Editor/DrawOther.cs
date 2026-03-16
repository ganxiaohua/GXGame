#if UNITY_EDITOR
using System.Diagnostics;
using GamePlay.Runtime.MapData;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static class DrawOther
    {
        public static GUIStyle GUISkin;

        static DrawOther()
        {
            GUISkin = new GUIStyle();
            GUISkin ??= new GUIStyle()
            {
                    normal = new GUIStyleState() {textColor = Color.black},
                    fontSize = 12,
            };
        }

        [Conditional("UNITY_EDITOR")]
        public static void DrawArea(AreaData areaData)
        {
            var old = Handles.matrix;
            int x = (int) (areaData.Size.x / areaData.ChunkSize.x) + (areaData.Size.x % areaData.ChunkSize.x != 0 ? 1 : 0);
            int y = (int) (areaData.Size.y / areaData.ChunkSize.y) + (areaData.Size.y % areaData.ChunkSize.y != 0 ? 1 : 0);
            for (int i = 0; i < y; i++)
            {
                for (int j = 0; j < x; j++)
                {
                    var size = areaData.ChunkSize.XZCoordinateCVector3();
                    Vector3 pos = areaData.StartPoint.XZCoordinateCVector3() + new Vector3(j * areaData.ChunkSize.x, 0, i * areaData.ChunkSize.y);
                    Handles.matrix = Matrix4x4.TRS(pos + size / 2, Quaternion.identity, Vector3.one);
                    Handles.color = Color.cyan;
                    Handles.DrawWireCube(Vector3.zero, size);
                    Handles.Label(Vector3.zero, $"{i * areaData.CellSize.x + j}", GUISkin);
                }
            }

            Handles.matrix = old;
        }
    }
}
#endif