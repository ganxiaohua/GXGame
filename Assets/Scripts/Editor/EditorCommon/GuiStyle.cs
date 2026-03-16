using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    public static class GuiStyle
    {
        private static GUIStyle guiStyle;
        private static GUIStyle wfontStyle;

        public static GUIStyle GetComGuiStytle()
        {
            return guiStyle ??= new GUIStyle(EditorStyles.label)
            {
                    alignment = TextAnchor.MiddleCenter,
                    fontSize = 10
            };
        }
    }
}