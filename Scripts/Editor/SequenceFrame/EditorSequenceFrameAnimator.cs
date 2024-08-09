using System;
using System.Collections.Generic;
using GameFrame.Editor;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

namespace GXGame.Editor
{
    public class EditorSequenceFrameAnimator : ScriptableObject
    {
        private static float Frame = 30;
        private static string OutPutPath = "Assets/GXGame/Art/Runtime/Role";

        [Serializable]
        public struct Data
        {
            private string GroupTitle;

            [FoldoutGroup("$GroupTitle", 1)] [LabelText("目标图片")]
            public Texture2D RooteTexture2D;

            [FoldoutGroup("$GroupTitle", 2)] [LabelText("图片单位格宽高")]
            public List<Vector2Int> WhCountList;

            [FoldoutGroup("$GroupTitle", 3)] [LabelText("图片需要生成的Clip")]
            public List<string> AimationClipName;

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

                if (allAnimationCount != AimationClipName.Count)
                {
                    Debug.Log("名字列表的长度必须和WHCount的y值一样");
                    return;
                }

                string assetPath = AssetDatabase.GetAssetPath(RooteTexture2D);
                string textureName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                OpFile.DeleteFilesInDirectory($"{OutPutPath}/{textureName}");
                AssetDatabase.Refresh();
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
                        CreateSpriteClip(list, $"{OutPutPath}/{textureName}/Animation/{AimationClipName[animIndex++]}.anim");
                    }
                }

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }


            private void CreateSpriteClip(List<Sprite> sprites, string path)
            {
                OpFile.CreateDiectory(path);
                AnimationClip clip = new AnimationClip();
                EditorCurveBinding curveBinding = new EditorCurveBinding();
                curveBinding.type = typeof(SpriteRenderer);
                curveBinding.path = "";
                curveBinding.propertyName = "m_Sprite";
                ObjectReferenceKeyframe[] keyframes = new ObjectReferenceKeyframe[sprites.Count];
                for (int i = 0; i < sprites.Count; i++)
                {
                    keyframes[i] = new ObjectReferenceKeyframe();
                    keyframes[i].time = i * 5 / Frame;
                    keyframes[i].value = sprites[i];
                }

                clip.frameRate = Frame;
                AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(clip);
                clipSettings.loopTime = true;
                AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
                AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keyframes);
                AssetDatabase.CreateAsset(clip, path);
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
            private void AnimatorTransition()
            {
                string assetPath = AssetDatabase.GetAssetPath(RooteTexture2D);
                string textureName = System.IO.Path.GetFileNameWithoutExtension(assetPath);
                CreateAnimtorController($"{OutPutPath}/{textureName}/Animation/{textureName}.controller", $"{OutPutPath}/{textureName}/Animation");


                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();
            }

            private void CreateAnimtorController(string path, string rolePath)
            {
                OpFile.CreateDiectory(path);
                AssetDatabase.DeleteAsset(path);
                AnimatorController animator = new AnimatorController();
                animator.AddLayer("Layer_1");
                var animatorControllerParameter = new AnimatorControllerParameter();
                animatorControllerParameter.name = "Stop";
                animatorControllerParameter.type = AnimatorControllerParameterType.Bool;
                animator.AddParameter(animatorControllerParameter);

                for (int i = 0; i < LinkCount; i++)
                {
                    LinkState(animator.layers[0].stateMachine, rolePath, i);
                }

                for (int i = LinkCount*2; i < AimationClipName.Count; i++)
                {
                    AddState(animator.layers[0].stateMachine, rolePath, i);
                }
                
                AssetDatabase.CreateAsset(animator, path);
                CreatePrefab(animator);
            }

            private void LinkState(AnimatorStateMachine machine, string rolePath, int index)
            {
                var clipHead = AssetDatabase.LoadAssetAtPath<AnimationClip>($"{rolePath}/{AimationClipName[index + IntervalCount]}.anim");
                var clipEnd = AssetDatabase.LoadAssetAtPath<AnimationClip>($"{rolePath}/{AimationClipName[index]}.anim");
                AnimatorState state1 = new AnimatorState();
                state1.motion = clipHead;
                state1.name = clipHead.name;

                AnimatorState state4 = new AnimatorState();
                state4.motion = clipEnd;
                state4.name = clipEnd.name;

                AnimatorStateTransition animatorStateTransition = state1.AddTransition(state4);
                animatorStateTransition.hasExitTime = false;
                List<AnimatorCondition> animatorConditionlist = new();
                AnimatorCondition animatorCondition = new AnimatorCondition();
                animatorCondition.parameter = "Stop";
                animatorCondition.mode = AnimatorConditionMode.If;
                animatorConditionlist.Add(animatorCondition);
                animatorStateTransition.conditions = animatorConditionlist.ToArray();

                machine.AddState(state1, new Vector3(250, index * 100));
                machine.AddState(state4, new Vector3(550, index * 100));
            }

            private void AddState(AnimatorStateMachine machine, string rolePath, int index)
            {
                var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>($"{rolePath}/{AimationClipName[index]}.anim");
                AnimatorState state1 = new AnimatorState();
                state1.motion = clip;
                state1.name = clip.name;
                machine.AddState(state1, new Vector3(250, (index-LinkCount) * 100));
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
                AssetDatabase.SaveAssets();
            }
        }

        [ShowInInspector] [ListDrawerSettings(NumberOfItemsPerPage = 20)]
        public List<Data> SequenceFrameAnimator = new List<Data>();
    }
}