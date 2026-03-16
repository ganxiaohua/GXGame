#if UNITY_EDITOR
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class MapArea
    {
        [ShowInInspector]
        public Vector3 WorldVector2Editor;

        private void DrawArea()
        {
            if (areaData == null)
                return;
            DrawOther.DrawArea(areaData);
            var moveRect = this.moveAreaRect;
            moveRect.position += areaData.StartPoint;
            Handles.color = Color.red;
            Handles.DrawPolyLine(
                    new Vector3(moveRect.xMin, 1, moveRect.yMin + moveRect.size.y),
                    new Vector3(moveRect.xMin, 1, moveRect.yMin),
                    new Vector3(moveRect.xMin + moveRect.size.x, 1, moveRect.yMin),
                    new Vector3(moveRect.xMin + moveRect.size.x, 1, moveRect.yMin + moveRect.size.y),
                    new Vector3(moveRect.xMin, 1, moveRect.yMin + moveRect.size.y));
            var areaRect = this.showAreaRect;
            areaRect.position += areaData.StartPoint;
            Handles.color = Color.yellow;
            Handles.DrawPolyLine(
                    new Vector3(areaRect.xMin, 1, areaRect.yMin + areaRect.size.y),
                    new Vector3(areaRect.xMin, 1, areaRect.yMin),
                    new Vector3(areaRect.xMin + areaRect.size.x, 1, areaRect.yMin),
                    new Vector3(areaRect.xMin + areaRect.size.x, 1, areaRect.yMin + areaRect.size.y),
                    new Vector3(areaRect.xMin, 1, areaRect.yMin + areaRect.size.y));
        }
    }
}
#endif