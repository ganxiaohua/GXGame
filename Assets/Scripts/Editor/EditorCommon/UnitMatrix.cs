using System;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using GameFrame.Editor;
using GamePlay.Editor.Data;
using GamePlay.Runtime;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GamePlay.Editor
{
    public struct UnitData
    {
        [HideInInspector]
        public Action<int> SelectAction;

        [ShowInInspector] [HorizontalGroup("unit")] [GUIColor("Color")] [ReadOnly]
        public int id;

        [ShowInInspector] [HorizontalGroup("unit")] [GUIColor("Color")] [ReadOnly]
        public string doc;

        [HideInInspector]
        public Color Color;

        [ShowInInspector]
        [HorizontalGroup("unit")]
        [GUIColor("Color")]
        public void Select()
        {
            SelectAction(id);
        }
    }

    [HideReferenceObjectPicker]
    public class UnitMatrix
    {
        [HideInInspector]
        public GameObject selectedPrefab;

        [HideInInspector]
        public GameObject selectGameObject;

        class PreTexture2d
        {
            public Texture2D PreTexture2D;
        }

        // 添加一个字典来缓存预览纹理
        private System.Collections.Generic.Dictionary<GameObject, PreTexture2d> previewCache = new();

        [Sirenix.OdinInspector.ShowIf("@prefabMatrix!=null")]
        [TableMatrix(HorizontalTitle = "展示区", RowHeight = 100, DrawElementMethod = "DrawColoredEnumElement")]
        [ShowInInspector]
        [PropertyOrder(5)]
        private GameObject[,] prefabMatrix;

        [HorizontalGroup("base", 200, 0, 0, 3)] [ShowInInspector] [PropertyOrder(1)] [OnValueChanged("SetBaseData")]
        public Vector3 scale = Vector3.one;

        [HorizontalGroup("base", 200)]
        [LabelText("scale")]
        [ShowInInspector]
        [PropertyOrder(2)]
        private void ScaleNor() => scale = Vector3.one;

        [HorizontalGroup("base", 200, 0, 0, 3)] [ShowInInspector] [PropertyOrder(3)] [OnValueChanged("SetBaseData")]
        public Vector3 rot = Vector3.zero;

        [HorizontalGroup("base", 200, 0, 0, 3)]
        [LabelText("scale")]
        [ShowInInspector]
        [PropertyOrder(4)]
        private void RotNor() => scale = Vector3.zero;

        [PropertyOrder(7)]
        [ShowIf("@selectedPrefab!=null")]
        [ShowInInspector]
        [ListDrawerSettings(HideAddButton = true, HideRemoveButton = true, DraggableItems = true)]
        private List<UnitData> unitDataList = new();

        private HashSet<string> showPaths;

        [HideInInspector]
        public int selectUnitId;

        private Action OnSelect;

        public void Init(Action action)
        {
            selectUnitId = 0;
            OnSelect = action;
        }

        public void Clear()
        {
            if (selectGameObject)
                Object.DestroyImmediate(selectGameObject);
            // 清理预览缓存
            foreach (var texture in previewCache.Values)
            {
                if (texture != null && texture.PreTexture2D != null)
                    Object.DestroyImmediate(texture.PreTexture2D);
            }

            previewCache.Clear();
            prefabMatrix = null;
        }

        public void SetDrawData(HashSet<string> showPaths, bool editorRes = true)
        {
            this.showPaths = showPaths;
            if (showPaths.Count == 0)
            {
                return;
            }

            selectedPrefab = null;
            if (selectGameObject != null)
                Object.DestroyImmediate(selectGameObject);
            prefabMatrix = null;
            if (showPaths.Count == 0)
                return;

            var guids = AssetDatabase.FindAssets("t:Prefab", showPaths.ToArray());
            List<string> paths = new List<string>();
            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                if (editorRes)
                {
                    List<int> unitList = TableEditorConst.GetUnitIdsWithModelName(YooAssetPath.GetAssetPath(path));
                    if (unitList != null)
                        paths.Add(path);
                }
                else
                {
                    paths.Add(path);
                }
            }

            int count = paths.Count + 1;
            int x = 8;
            int y = count / x + 1;
            prefabMatrix = new GameObject[x, y];
            prefabMatrix[0, 0] = null;
            for (int i = 0; i < paths.Count; i++)
            {
                var path = paths[i];
                var go = AssetDatabase.LoadAssetAtPath<GameObject>(path);
                int index = i + 1;
                prefabMatrix[index % x, index / x] = go;
            }
        }


        private GameObject DrawColoredEnumElement(Rect rect, GameObject go)
        {
            if (go != null)
            {
                if (!previewCache.TryGetValue(go, out var preTexture2d))
                {
                    // 创建预览图
                    preTexture2d = new PreTexture2d();
                    CreatePreview(go, preTexture2d);
                    previewCache[go] = preTexture2d;
                }

                // 绘制预览图
                if (preTexture2d.PreTexture2D != null)
                {
                    // 计算居中绘制的位置
                    float size = Mathf.Min(rect.width, rect.height) * 0.8f;
                    float x = rect.x + (rect.width - size) / 2;
                    float y = rect.y + (rect.height - size) / 2;

                    // 绘制预览图
                    GUI.DrawTexture(new Rect(x, y, size, size), preTexture2d.PreTexture2D, ScaleMode.ScaleToFit);
                }
                else
                {
                    // 如果没有预览图，则绘制图标
                    GUIContent content = EditorGUIUtility.ObjectContent(go, typeof(GameObject));
                    if (content.image != null)
                    {
                        float size = Mathf.Min(rect.width, rect.height) * 0.8f;
                        float x = rect.x + (rect.width - size) / 2;
                        float y = rect.y + (rect.height - size) / 2;
                        GUI.DrawTexture(new Rect(x, y, size, size), content.image, ScaleMode.ScaleToFit);
                    }
                }

                // 如果这是当前选中的GameObject，绘制白色边框
                if (go == selectedPrefab)
                {
                    Color borderColor = Color.white;
                    float borderThickness = 2f;

                    // 绘制四条边框线
                    EditorGUI.DrawRect(new Rect(rect.x, rect.y, rect.width, borderThickness), borderColor);
                    EditorGUI.DrawRect(new Rect(rect.x, rect.y, borderThickness, rect.height), borderColor);
                    EditorGUI.DrawRect(new Rect(rect.xMax - borderThickness, rect.y, borderThickness, rect.height), borderColor);
                    EditorGUI.DrawRect(new Rect(rect.x, rect.yMax - borderThickness, rect.width, borderThickness), borderColor);
                }

                var rectTest = new Rect();
                rectTest = rect;
                rectTest.position += new Vector2(0, 40);
                EditorGUI.LabelField(rectTest, go.name, GuiStyle.GetComGuiStytle());
            }

            // 处理鼠标点击事件
            if (Event.current.type == EventType.MouseDown && rect.Contains(Event.current.mousePosition))
            {
                OnSelect?.Invoke();
                if (selectedPrefab != go)
                {
                    unitDataList.Clear();
                    selectUnitId = 0;
                    selectedPrefab = go;
                    ChangeViewObj();
                }

                if (selectedPrefab != null)
                {
                    List<int> unitList = TableEditorConst.GetUnitIdsWithModelName(YooAssetPath.GetAssetPath(AssetDatabase.GetAssetPath(selectedPrefab)));
                    if (unitList != null && unitList.Count != 0)
                    {
                        foreach (var unitId in unitList)
                        {
                            var item = Tables.Instance.UnitTable.Get(unitId);
                            unitDataList.Add(new UnitData() {id = item.Id, Color = Color.white, doc = "", SelectAction = UnitListSelect});
                        }

                        UnitListSelect(unitList[0]);
                    }
                }

                GUI.changed = true;
                Event.current.Use();
            }

            return go;
        }

        private void UnitListSelect(int unitId)
        {
            for (int i = 0; i < unitDataList.Count; i++)
            {
                var unit = unitDataList[i];
                if (unit.id == unitId)
                {
                    unitDataList[i] = new UnitData() {id = unit.id, Color = Color.cyan, doc = unit.doc, SelectAction = UnitListSelect};
                    selectUnitId = unit.id;
                }
                else
                {
                    unitDataList[i] = new UnitData() {id = unit.id, Color = Color.white, doc = unit.doc, SelectAction = UnitListSelect};
                }
            }
        }

        public void NoSelect()
        {
            selectUnitId = 0;
            selectedPrefab = null;
            ChangeViewObj();
        }

        private void ChangeViewObj()
        {
            if (selectGameObject != null)
                Object.DestroyImmediate(selectGameObject);
            if (selectedPrefab != null)
            {
                selectGameObject = PrefabUtility.InstantiatePrefab(selectedPrefab) as GameObject;
                var colliders = selectGameObject.GetComponentsInChildren<Collider>();
                SetBaseData();
                selectGameObject.hideFlags = HideFlags.HideInHierarchy;
                foreach (var collider in colliders)
                {
                    GameObject.DestroyImmediate(collider);
                }
            }
        }

        private void SetBaseData()
        {
            if (selectGameObject == null)
                return;
            selectGameObject.transform.localScale = scale;
            selectGameObject.transform.rotation = Quaternion.Euler(rot);
        }

        private void CreatePreview(GameObject go, PreTexture2d texture)
        {
            if (go == null) return;

            try
            {
                EditorUtility.SetDirty(go);
                texture.PreTexture2D = AssetPreview.GetAssetPreview(go);
                // while (true)
                // {
                //     bool succ = AssetPreview.IsLoadingAssetPreview(go.GetInstanceID());
                //     if (!succ)
                //         await UniTask.Yield();
                //     else
                //     {
                //         texture.PreTexture2D = AssetPreview.GetAssetPreview(go);
                //         break;
                //     }
                // }
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("无法为 " + go.name + " 创建预览图: " + e.Message);
            }
        }
    }
}