using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor
{
    public partial class GridManager
    {
        private Material cursorMaterial;
        private Mesh cursor;
        private Vector2Int curCursorPos;
        private Vector2Int lastCursorPos;
        private bool buttonDown;

        public void GetCursorPosWithEditorScene(SceneView sceneView)
        {
            Event e = Event.current;
            Vector2 mousePos = e.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePos);
            Plane groundPlane = new Plane(Vector3.up, GridData.CroplandData.Pos);
            if (groundPlane.Raycast(ray, out float enter))
            {
                Vector3 worldPos = ray.GetPoint(enter);
                curCursorPos = GridData.CroplandData.WorldToCell(worldPos);
                worldPos.y += 1.0f;
                guiStyle.normal.textColor = Color.magenta;
                Handles.Label(worldPos, $"Cell Position: {curCursorPos}", guiStyle);
                // MouseArea();
                // InputArea();
                guiStyle.normal.textColor = Color.white;
            }
        }

        // private void MouseArea()
        // {
        //     ClearCursor();
        //     RectInt rect = new RectInt(curCursorPos.x, curCursorPos.y, GridData.brushSize.x, GridData.brushSize.y);
        //     if (GridData.InArea(rect))
        //     {
        //         DrawGrid.CreateGridMesh(GridData, new Vector3(0, 0.1f, 0) + offset, rect, ref cursor, ref cursorMaterial, Color.green);
        //     }
        // }

        // private void InputArea()
        // {
        //     var e = Event.current;
        //     if (e.OnMouseUp(0))
        //     {
        //         buttonDown = true;
        //         cz();
        //     }
        //     else if (e.OnMouseUp(0))
        //     {
        //         buttonDown = false;
        //     }
        //
        //     if (e.OnMouseMoveDrag() && buttonDown)
        //     {
        //         cz();
        //     }
        //
        //     void cz()
        //     {
        //         int controlID = GUIUtility.GetControlID(FocusType.Passive);
        //         RectInt rect = new RectInt(curCursorPos.x, curCursorPos.y, GridData.brushSize.x, GridData.brushSize.y);
        //         if (!GridData.IsClear)
        //             GridData.AddObstacle(rect);
        //         else
        //             GridData.RemoveObstacle(rect);
        //         Event.current.Use();
        //         GUIUtility.hotControl = controlID;
        //     }
    }
    //
    // private void ClearCursor()
    // {
    //     if (cursor != null)
    //     {
    //         DestroyImmediate(cursor);
    //         cursor = null;
    //     }
    // }
// }
}