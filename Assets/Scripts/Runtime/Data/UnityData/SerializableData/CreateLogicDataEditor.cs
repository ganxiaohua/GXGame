#if UNITY_EDITOR
using System.IO;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class CreateLogicDataEditor : MonoBehaviour
    {
        private string PathDetection()
        {
            string fileName = AssetDatabase.GetAssetPath(gameObject);
            fileName = fileName.Replace("Assets/ResEditor", "Assets/Res");
            string path = Path.GetDirectoryName(fileName);
            if (!Directory.Exists(path))
                Directory.CreateDirectory(Path.GetDirectoryName(fileName) ?? string.Empty);
            return path;
        }

        [Button]
        public void CollectColliderInfo()
        {
            string assetPath = PathDetection();
            GameObject obj = Instantiate(gameObject);
            DestroyImmediate(obj.GetComponent<CreateLogicDataEditor>());
            obj.name = gameObject.name;
            assetPath += $"/{obj.name}";
            CreateCollider(obj, assetPath);
            Collider[] colliders = obj.GetComponents<Collider>();
            foreach (Collider collider in colliders)
            {
                DestroyImmediate(collider);
            }

            PrefabUtility.SaveAsPrefabAsset(obj, assetPath + ".prefab");
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            DestroyImmediate(obj);
        }

        private void CreateCollider(GameObject obj, string assetPath)
        {
            Collider[] colliders = obj.GetComponentsInChildren<Collider>(true);
            LogicData info = ScriptableObject.CreateInstance<LogicData>();
            info.Type = LogicData.ColliderEnum.None;
            if (colliders.Length > 1)
            {
                Debug.LogWarning("碰撞体数量超过一个!");
                return;
            }

            info.Pos = obj.transform.position;
            info.Rot = obj.transform.rotation;
            info.Scale = obj.transform.localScale;
            foreach (Collider col in colliders)
            {
                info.Layer = obj.layer;
                info.IsTrigger = col.isTrigger;
                // 根据不同类型获取特定属性
                if (col is BoxCollider box)
                {
                    info.Type = LogicData.ColliderEnum.BoxCollider;
                    info.Center = box.center;
                    info.Size = box.size;
                }
                else if (col is SphereCollider sphere)
                {
                    info.Type = LogicData.ColliderEnum.SphereCollider;
                    info.Center = sphere.center;
                    info.Radius = sphere.radius;
                }
                else if (col is CapsuleCollider capsule)
                {
                    info.Type = LogicData.ColliderEnum.CapsuleCollider;
                    info.Center = capsule.center;
                    info.Radius = capsule.radius;
                    info.Height = capsule.height;
                    info.Direction = capsule.direction;
                }
                if (!col.enabled)
                    info.Type = LogicData.ColliderEnum.None;
            }

            AssetDatabase.CreateAsset(info, assetPath + "_Logic.asset");
        }
    }
}
#endif