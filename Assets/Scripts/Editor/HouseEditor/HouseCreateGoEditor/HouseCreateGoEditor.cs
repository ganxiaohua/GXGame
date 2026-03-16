using Sirenix.OdinInspector;
using Sirenix.OdinInspector.Editor;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Editor.MapEditor
{
    public partial class HouseCreateGoEditor : OdinEditorWindow, IHouseEditorSceneUpdate
    {
        public bool isClear { get; private set; }
        private static HouseCreateGoEditor window;
        private HouseEditor houseEditor;
        
        [PropertyOrder(2)]
        [ShowInInspector]
        private UnitMatrix unitMatrix;

        private EditorGos editorGos;

        private bool showGameObjectButton;


        public static HouseCreateGoEditor Create()
        {
            window = ScriptableObject.CreateInstance<HouseCreateGoEditor>();
            return window;
        }

        public void Init(HouseEditor mapEditor)
        {
            houseEditor = mapEditor;
            unitMatrix = new UnitMatrix();
            unitMatrix.Init(null);
            InitFolder();
            isClear = false;
            editorGos = new EditorGos();
            editorGos.Init(RemoveUnit);
            showGameObjectButton = false;
        }

        public void Clear()
        {
            if (isClear)
                return;
            unitMatrix?.Clear();
            ClearFolder();
            Freed();
            isClear = true;
        }

        private void Freed()
        {
            editorGos.Clear();
            if (houseEditor.House != null)
                GameObject.DestroyImmediate(houseEditor.House);
        }

        public void OnSceneUpdate(SceneView view)
        {
            editorGos.OnScene();
            OnScene();
        }

        private void OnScene()
        {
            Event e = Event.current;
            Ray ray = HandleUtility.GUIPointToWorldRay(e.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                CreateGo(e, hit);
            }
            else
            {
                if (unitMatrix.selectedPrefab == null || unitMatrix.selectGameObject == null || houseEditor.House == null)
                    return;
                unitMatrix.selectGameObject.SetActive(false);
            }

            OtherEvent(e);
        }

        private void OtherEvent(Event e)
        {
            if (e != null && e.type == EventType.KeyDown)
            {
                if ((e.control || e.command) && e.keyCode == KeyCode.S)
                {
                    if (houseEditor != null)
                    {
                        EditorUtility.SetDirty(houseEditor.HouseDataData);
                        AssetDatabase.SaveAssets();
                    }
                }
                else if (e.alt)
                {
                    showGameObjectButton = !showGameObjectButton;
                    editorGos.ShowButton = showGameObjectButton;
                    e.Use();
                }
            }
        }

        public void CreateGo(Event e, RaycastHit hit)
        {
            Vector3 worldPosition = hit.point;
            if (unitMatrix.selectedPrefab == null || unitMatrix.selectGameObject == null || houseEditor.House == null)
                return;
            unitMatrix.selectGameObject.SetActive(true);
            unitMatrix.selectGameObject.transform.position = worldPosition;
            if (e.type == EventType.MouseDown && e.button == 0)
            {
                InjectGo(worldPosition);
                e.Use();
            }
        }
    }
}