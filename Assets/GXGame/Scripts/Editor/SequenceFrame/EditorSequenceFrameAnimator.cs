using System;
using System.Collections.Generic;
using GameFrame.Runtime.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace GXGame.Editor
{
    public partial class EditorSequenceFrameAnimator : ScriptableObject
    {
        private static float Frame = 30;
        private static string OutPutPath = "Assets/GXGame/Art/Runtime/Role";

        [Serializable]
        public struct Data
        {
            public enum AnimatorType
            {
                BelowWalk,
                BelowIdle,
                RightWalk,
                RightIdle,
                ForwardWalk,
                ForwordIdle,
                BelowAtk,
                RightAtk,
                ForwardAtk,
                Die
            }

            private string GroupTitle;

            [FoldoutGroup("$GroupTitle", 1)] [LabelText("目标图片")]
            public Texture2D RooteTexture2D;

            [FoldoutGroup("$GroupTitle", 2)] [LabelText("图片单位格宽高")]
            public List<Vector2Int> WhCountList;

            [FormerlySerializedAs("AimationClipName")] [FoldoutGroup("$GroupTitle", 3)] [LabelText("图片需要生成的Clip")]
            public List<AnimatorType> AimationClipType;

            [Button]
            [FoldoutGroup("$GroupTitle", 4)]
            [LabelText("生成动画")]
            public void CuttingSprite()
            {
                int allAnimationCount = 0;
                foreach (var whCount in WhCountList)
                {
                    allAnimationCount += whCount.y;
                }

                if (allAnimationCount != AimationClipType.Count)
                {
                    Debug.Log("名字列表的长度必须和WHCount的y值一样");
                    return;
                }

                string assetPath = AssetDatabase.GetAssetPath(RooteTexture2D);
                string textureName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                OpFile.DeleteFilesInDirectory($"{OutPutPath}/{textureName}");
                AssetDatabase.Refresh();
                AssetDatabase.SaveAssets();
                GroupTitle = textureName;
                UnityEngine.Object[] sprites = AssetDatabase.LoadAllAssetRepresentationsAtPath(assetPath);
                List<Sprite> list = new List<Sprite>();
                int index = 0;
                int animIndex = 0;
                foreach (var whCount in WhCountList)
                {
                    for (int y = 0; y < whCount.y; y++)
                    {
                        list.Clear();
                        for (int x = 0; x < whCount.x; x++)
                        {
                            Sprite sprite = (Sprite) sprites[index++];
                            if (sprite != null)
                            {
                                list.Add(sprite);
                            }
                        }

                        if (list.Count == 0)
                            return;
                        CreateSpriteClip(list, $"{OutPutPath}/{textureName}/Animation/{AimationClipType[animIndex++].ToString()}.anim");
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            [FoldoutGroup("$GroupTitle", 5)] [LabelText("是否链接动画")]
            public bool IsTransition;

            [FoldoutGroup("$GroupTitle", 6)] [FormerlySerializedAs("intervalCount")] [LabelText("间隔数")] [ShowIf("IsTransition", true)]
            public int IntervalCount;

            [FormerlySerializedAs("linkCount")] [LabelText("连接的组数")] [ShowIf("IsTransition", true)] [FoldoutGroup("$GroupTitle", 7)]
            public int LinkCount;

            [Button]
            [FoldoutGroup("$GroupTitle", 8)]
            [LabelText("动画连接")]
            [ShowIf("IsTransition", true)]
            public void AnimatorTransition()
            {
                string assetPath = AssetDatabase.GetAssetPath(RooteTexture2D);
                string textureName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                var animtorConllor = CreateAnimtorController(AimationClipType, $"{OutPutPath}/{textureName}/Animation/{textureName}.controller",
                        $"{OutPutPath}/{textureName}/Animation");
                CreatePrefab(animtorConllor);
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            private void CreatePrefab(AnimatorController animatorController)
            {
                string assetPath = AssetDatabase.GetAssetPath(RooteTexture2D);
                string textureName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                string targetPath = $"{OutPutPath}/{textureName}/Prefab/{textureName}.prefab";
                OpFile.CreateDiectory(targetPath);
                AssetDatabase.DeleteAsset(targetPath);
                AssetDatabase.CopyAsset("Assets/GXGame/Art/Editor/Template/RoleTemplate.prefab", targetPath);
                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(targetPath);
                go.GetComponent<Animator>().runtimeAnimatorController = animatorController;
                EditorUtility.SetDirty(go);
            }
        }

        [ShowInInspector] [ListDrawerSettings(NumberOfItemsPerPage = 20)]
        public List<Data> SequenceFrameAnimator = new List<Data>();


        [LabelText("输出全部")]
        [Button]
        public void OutputAll()
        {
            foreach (var data in SequenceFrameAnimator)
            {
                data.CuttingSprite();
                data.AnimatorTransition();
            }
        }
    }
}