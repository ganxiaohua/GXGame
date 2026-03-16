#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace GameFrame.Runtime
{
    public static class DebugSceneView
    {
        struct DrawData
        {
            public Vector3 Pos;

            public Vector3 Size;

            public Quaternion Qua;

            public Color Color;

            public string Text;

            public float DestructionTime;
        }

        private static GUIStyle eguiStyle;

        private static Dictionary<object, DrawData> drawList = new();
        private static List<object> waitQueueDeleted = new List<object>();
        public static event Action Draw;

        public static void Init()
        {
            eguiStyle = new GUIStyle();
            eguiStyle.normal.textColor = Color.white;
            eguiStyle.fontSize = 12;
            eguiStyle.padding = new RectOffset(5, 5, 5, 5);
            UnityEditor.SceneView.duringSceneGui += OnSceneGuiDelegate;
        }

        public static void Clear()
        {
            UnityEditor.SceneView.duringSceneGui -= OnSceneGuiDelegate;
            UnityEditor.SceneView.RepaintAll();
        }

        [Conditional("ShowEditorLine")]
        public static void SetBox(object key, Vector3 pos, Quaternion qua, Vector3 size, Color color, string text)
        {
            drawList[key] = (new DrawData()
                    {Pos = pos, Size = size, Qua = qua, Color = color, Text = text, DestructionTime = Time.realtimeSinceStartup + 5});
        }


        private static void OnSceneGuiDelegate(UnityEditor.SceneView view)
        {
            if (!view.drawGizmos)
                return;
            var old = Handles.matrix;
            foreach (var drawKV in drawList)
            {
                var draw = drawKV.Value;
                Handles.matrix = Matrix4x4.TRS(draw.Pos, draw.Qua, Vector3.one);
                Handles.color = draw.Color;
                Handles.DrawWireCube(Vector3.zero, draw.Size);
                Handles.DrawWireCube(Vector3.zero + new Vector3(0.01f, 0, 0), draw.Size);
                Handles.Label(Vector3.zero, draw.Text, eguiStyle);
                if (draw.DestructionTime <= Time.realtimeSinceStartup)
                {
                    waitQueueDeleted.Add(drawKV.Key);
                }
            }

            foreach (var item in waitQueueDeleted)
            {
                if (item != null)
                    drawList.Remove(item);
            }

            waitQueueDeleted.Clear();
            Handles.matrix = old;
            Draw?.Invoke();
        }
    }
}
#endif