using System.Collections.Generic;
using GameFrame.Runtime.Editor;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;

namespace GXGame.Editor
{
    public partial class EditorSequenceFrameAnimator
    {
        private static void CreateSpriteClip(List<Sprite> sprites, string path)
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

        private static void CreateParameter(AnimatorController animator)
        {
            var animatorControllerParameter = new AnimatorControllerParameter();
            animatorControllerParameter.name = "Stop";
            animatorControllerParameter.type = AnimatorControllerParameterType.Bool;
            animator.AddParameter(animatorControllerParameter);

            animatorControllerParameter = new AnimatorControllerParameter();
            animatorControllerParameter.name = "Direction";
            animatorControllerParameter.type = AnimatorControllerParameterType.Int;
            animator.AddParameter(animatorControllerParameter);


            animatorControllerParameter = new AnimatorControllerParameter();
            animatorControllerParameter.name = "State";
            animatorControllerParameter.type = AnimatorControllerParameterType.Int;
            animator.AddParameter(animatorControllerParameter);
        }


        private static AnimatorController CreateAnimtorController(List<Data.AnimatorType> aimationClipType, string path, string rolePath)
        {
            OpFile.CreateDiectory(path);
            AssetDatabase.DeleteAsset(path);
            AnimatorController animator = new AnimatorController();
            AssetDatabase.CreateAsset(animator, path);
            AnimatorControllerLayer layer = new AnimatorControllerLayer();
            layer.blendingMode = AnimatorLayerBlendingMode.Override;
            layer.name = "Base Layer";
            layer.stateMachine = new AnimatorStateMachine();
            layer.stateMachine.name = layer.name;
            layer.stateMachine.hideFlags = HideFlags.HideInHierarchy;
            CreateParameter(animator);
            animator.AddLayer(layer);
            AssetDatabase.AddObjectToAsset(layer.stateMachine, animator);
            Dictionary<Data.AnimatorType, AnimatorState> typeDic = new();
            foreach (var clipType in aimationClipType)
            {
                var state = CreateState(animator, layer.stateMachine, rolePath, clipType);
                typeDic.Add(clipType, state);
            }

            Transition(animator, layer.stateMachine, typeDic);
            EditorUtility.SetDirty(animator);
            return animator;
        }


        private static AnimatorState CreateState(AnimatorController controller, AnimatorStateMachine machine, string rolePath, Data.AnimatorType animatorType)
        {
            var clip = AssetDatabase.LoadAssetAtPath<AnimationClip>($"{rolePath}/{animatorType.ToString()}.anim");
            AnimatorState state = new AnimatorState();
            state.hideFlags = HideFlags.HideInHierarchy;
            state.motion = clip;
            state.name = clip.name;
            AssetDatabase.AddObjectToAsset(state, controller);
            return state;
        }

        private static void Transition(AnimatorController controller, AnimatorStateMachine machine, Dictionary<Data.AnimatorType, AnimatorState> typeDic)
        {
            int pos = 0;
            if (typeDic.ContainsKey(Data.AnimatorType.BelowWalk) && typeDic.ContainsKey(Data.AnimatorType.BelowIdle))
            {
                AnimatorStateTransition(controller, machine, typeDic[Data.AnimatorType.BelowWalk], typeDic[Data.AnimatorType.BelowIdle], 1);
            }

            if (typeDic.ContainsKey(Data.AnimatorType.RightWalk) && typeDic.ContainsKey(Data.AnimatorType.RightIdle))
            {
                AnimatorStateTransition(controller, machine, typeDic[Data.AnimatorType.RightWalk], typeDic[Data.AnimatorType.RightIdle], 2);
            }

            if (typeDic.ContainsKey(Data.AnimatorType.ForwardWalk) && typeDic.ContainsKey(Data.AnimatorType.ForwordIdle))
            {
                AnimatorStateTransition(controller, machine, typeDic[Data.AnimatorType.ForwardWalk], typeDic[Data.AnimatorType.ForwordIdle], 3);
            }

            pos = 4;
            if (typeDic.TryGetValue(Data.AnimatorType.BelowAtk, out var value))
            {
                AnimatorStateTransition(controller, machine, value, pos, 2);
            }

            if (typeDic.TryGetValue(Data.AnimatorType.RightAtk, out var value2))
            {
                AnimatorStateTransition(controller, machine, value2, ++pos, 3);
            }

            if (typeDic.TryGetValue(Data.AnimatorType.ForwardAtk, out var value3))
            {
                AnimatorStateTransition(controller, machine, value3, ++pos, 4);
            }

            if (typeDic.TryGetValue(Data.AnimatorType.Die, out var value4))
            {
                AnimatorStateTransition(controller, machine, value4, ++pos, 5);
            }
        }

        private static void AnimatorStateTransition(AnimatorController controller, AnimatorStateMachine machine, AnimatorState a, AnimatorState b,
                int direction)
        {
            int pos = direction;
            machine.AddState(a, new Vector3(250, pos * 100));
            machine.AddState(b, new Vector3(550, pos * 100));
            List<AnimatorCondition> animatorConditionlist = new();
            AnimatorStateTransition animatorStateTransition = a.AddTransition(b);
            SetAnimatorStateTransition(animatorStateTransition);
            AnimatorCondition animatorCondition = new AnimatorCondition();
            animatorCondition.parameter = "Stop";
            animatorCondition.mode = AnimatorConditionMode.If;
            animatorConditionlist.Add(animatorCondition);
            animatorStateTransition.conditions = animatorConditionlist.ToArray();

            animatorConditionlist.Clear();
            animatorStateTransition = machine.AddAnyStateTransition(a);
            SetAnimatorStateTransition(animatorStateTransition);
            animatorCondition = new AnimatorCondition();
            animatorCondition.parameter = "Direction";
            animatorCondition.mode = AnimatorConditionMode.Equals;
            animatorCondition.threshold = direction;
            animatorConditionlist.Add(animatorCondition);

            animatorCondition = new AnimatorCondition();
            animatorCondition.parameter = "State";
            animatorCondition.mode = AnimatorConditionMode.Equals;
            animatorCondition.threshold = 1;
            animatorConditionlist.Add(animatorCondition);

            animatorCondition = new AnimatorCondition();
            animatorCondition.parameter = "Stop";
            animatorCondition.mode = AnimatorConditionMode.IfNot;
            animatorConditionlist.Add(animatorCondition);
            animatorStateTransition.conditions = animatorConditionlist.ToArray();
        }

        private static void AnimatorStateTransition(AnimatorController controller, AnimatorStateMachine machine, AnimatorState a, int pos, int state)
        {
            List<AnimatorCondition> animatorConditionlist = new();
            machine.AddState(a, new Vector3(250, pos * 100));
            var animatorStateTransition = machine.AddAnyStateTransition(a);
            SetAnimatorStateTransition(animatorStateTransition);
            var animatorCondition = new AnimatorCondition();
            animatorCondition.parameter = "State";
            animatorCondition.mode = AnimatorConditionMode.Equals;
            animatorCondition.threshold = state;
            animatorConditionlist.Add(animatorCondition);
            animatorStateTransition.conditions = animatorConditionlist.ToArray();
        }

        private static void SetAnimatorStateTransition(AnimatorStateTransition animatorStateTransition)
        {
            animatorStateTransition.hideFlags = HideFlags.HideInHierarchy;
            animatorStateTransition.canTransitionToSelf = false;
            animatorStateTransition.hasExitTime = false;
            animatorStateTransition.hasFixedDuration = false;
            animatorStateTransition.duration = 0;
            animatorStateTransition.offset = 0;
            animatorStateTransition.exitTime = 0;
        }
    }
}