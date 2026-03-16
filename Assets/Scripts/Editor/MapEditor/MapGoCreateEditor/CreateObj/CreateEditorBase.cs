using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public class CreateEditorBase
    {
        [ShowInInspector]
        [PropertyOrder(2)]
        protected UnitMatrix unitMatrix;

        protected EditorGos editorGos;

        protected MapEditor mapEditor;

        protected MapGoCreateEditor mapGoCreateEditor;

        public virtual void Init(MapEditor mapEditor, MapGoCreateEditor mapGoCreateEditor)
        {
            this.mapEditor = mapEditor;
            this.mapGoCreateEditor = mapGoCreateEditor;
            unitMatrix = new UnitMatrix();
            unitMatrix.Init(NoSelectObj);
            editorGos = new EditorGos();
            editorGos.Init(RemoveUnit);
        }

        public virtual void Clear()
        {
            unitMatrix?.Clear();
            editorGos?.Clear();
            unitMatrix = null;
            editorGos = null;
        }


        public void ShowButton(bool show)
        {
            editorGos.ShowButton = show;
        }

        public void NoSelect()
        {
            unitMatrix.NoSelect();
        }

        public void OnScene(bool rayCast, Event e, RaycastHit raycat)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                CreateGo(e, hit);
            }

            editorGos.OnScene();
        }

        public void CreateGo(Event e, RaycastHit hit)
        {
            Vector3 worldPosition = hit.point;
            if (unitMatrix.selectedPrefab == null || unitMatrix.selectGameObject == null || mapEditor.AreaData == null)
                return;
            unitMatrix.selectGameObject.transform.position = worldPosition;
            Handles.Label(worldPosition, worldPosition.ToString(), GuiStyle.GetComGuiStytle());
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                InjectGo(worldPosition);
                e.Use();
            }
        }

        protected virtual void InjectGo(Vector3 worldPosition)
        {
        }

        protected virtual void RemoveUnit(int chunkIndex, int index, bool isPortal)
        {
        }

        protected virtual void NoSelectObj()
        {
        }
    }
}