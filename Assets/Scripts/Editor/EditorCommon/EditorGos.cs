using System;
using System.Collections.Generic;
using GamePlay.Runtime;
using GamePlay.Runtime.MapData;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace GamePlay.Editor
{
    public class EditorGosGOEx
    {
        public bool isPortal;
        public int ParentId;
        public GameObject Go;
    }

    public class EditorGos
    {
        private Dictionary<int, List<EditorGosGOEx>> chunkBaseGo;
        private Dictionary<int, List<EditorGosGOEx>> chunkPortal;
        private Action<int, int, bool> removeAction;
        public bool ShowButton;

        public void Init(Action<int, int, bool> doAction)
        {
            ShowButton = false;
            chunkBaseGo = new();
            chunkPortal = new();
            this.removeAction -= this.removeAction;
            this.removeAction += doAction;
            this.removeAction += RemoveUnit;
        }

        public void Clear()
        {
            if (chunkBaseGo != null)
                foreach (var items in chunkBaseGo)
                {
                    foreach (var item in items.Value)
                    {
                        if (item.Go)
                            Object.DestroyImmediate(item.Go);
                    }
                }

            if (chunkPortal != null)
            {
                foreach (var items in chunkPortal)
                {
                    foreach (var item in items.Value)
                    {
                        if (item.Go)
                            Object.DestroyImmediate(item.Go);
                    }
                }
            }

            chunkPortal.Clear();
            chunkBaseGo.Clear();
        }

        public GameObject AddUnitGo<T>(int parentId, Transform parent, string path, BaseRST rst, Vector3 lpos, Vector3 lscale, Quaternion lrotation) where T : EditorUnit
        {
            var goPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(path);
            return AddUnitGo<T>(parentId, parent, goPrefab, rst, lpos, lscale, lrotation);
        }

        public GameObject AddUnitGo<T>(int parentId, Transform parent, GameObject go, BaseRST rst, Vector3 lpos, Vector3 lscale, Quaternion lrotation) where T : EditorUnit
        {
            var newObject = PrefabUtility.InstantiatePrefab(go) as GameObject;
            if (parent != null)
                newObject.transform.SetParent(parent);
            newObject.transform.localPosition = lpos;
            newObject.transform.localScale = lscale;
            newObject.transform.localRotation = lrotation;
            newObject.hideFlags = HideFlags.DontSave;
            bool isProtal = typeof(T) == typeof(PortalEditorUnit);
            //newObject.hideFlags = HideFlags.HideAndDontSave | HideFlags.HideInInspector;
            List<EditorGosGOEx> goList;
            if (!isProtal)
            {
                chunkBaseGo ??= new();
                if (!chunkBaseGo.TryGetValue(parentId, out goList))
                {
                    goList = new List<EditorGosGOEx>();
                    chunkBaseGo[parentId] = goList;
                }
            }
            else
            {
                chunkPortal ??= new();
                if (!chunkPortal.TryGetValue(parentId, out goList))
                {
                    goList = new List<EditorGosGOEx>();
                    chunkPortal[parentId] = goList;
                }
            }

            goList.Add(new EditorGosGOEx() {ParentId = parentId, Go = newObject, isPortal = typeof(T) == typeof(PortalEditorUnit)});
            var editorUnit = newObject.AddComponent<T>();
            editorUnit.Init(rst, null);
            return newObject;
        }

        public void RemoveChunk(int parentId)
        {
            List<EditorGosGOEx> temp = new List<EditorGosGOEx>();
            if (chunkBaseGo.TryGetValue(parentId, out var goExList))
            {
                foreach (var go in goExList)
                {
                    temp.Add(go);
                }
            }

            if (chunkPortal.TryGetValue(parentId, out var goExList2))
            {
                foreach (var go in goExList2)
                {
                    temp.Add(go);
                }
            }

            for (int i = temp.Count - 1; i >= 0; i--)
            {
                RemoveUnit(temp[i]);
            }

            chunkBaseGo.Remove(parentId);
            chunkPortal.Remove(parentId);
        }

        public void RemoveUnit(EditorGosGOEx goEx)
        {
            Object.DestroyImmediate(goEx.Go);
            if (!goEx.isPortal)
                chunkBaseGo[goEx.ParentId].Remove(goEx);
            else
            {
                chunkPortal[goEx.ParentId].Remove(goEx);
            }
        }

        public void RemoveUnit(int parentID, int index, bool isPortal)
        {
            if (!isPortal)
            {
                var go = chunkBaseGo[parentID][index];
                Object.DestroyImmediate(go.Go);
                chunkBaseGo[parentID].RemoveAt(index);
            }
            else
            {
                var go = chunkPortal[parentID][index];
                Object.DestroyImmediate(go.Go);
                chunkPortal[parentID].RemoveAt(index);
            }
        }

        public void OnScene()
        {
            if (ShowButton)
            {
                foreach (var goList in chunkBaseGo)
                {
                    var list = goList.Value;
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        GameObjectButton(list[i]);
                    }
                }

                foreach (var goList in chunkPortal)
                {
                    var list = goList.Value;
                    for (int i = list.Count - 1; i >= 0; i--)
                    {
                        GameObjectButton(list[i]);
                    }
                }
            }
        }

        private void GameObjectButton(EditorGosGOEx goEx)
        {
            if (goEx != null)
            {
                Vector3 worldPosition = goEx.Go.transform.position;
                Vector2 guiPosition = HandleUtility.WorldToGUIPoint(worldPosition);
                Handles.BeginGUI();
                Rect buttonRect = new Rect(guiPosition.x - 15, guiPosition.y - 10, 30, 20);
                if (GUI.Button(buttonRect, "删"))
                {
                    int index = goEx.isPortal ? chunkPortal[goEx.ParentId].IndexOf(goEx) : chunkBaseGo[goEx.ParentId].IndexOf(goEx);
                    removeAction?.Invoke(goEx.ParentId, index, goEx.isPortal);
                    RemoveUnit(goEx);
                }

                Handles.EndGUI();
            }
        }
    }
}