using UnityEditor;
using UnityEngine;
namespace GXGame.Editor
{
    public static class CreateSequenceFrameAnimator
    {
        [MenuItem("Assets/Create/2DAnimator/Animator", priority = 1)]
        static void CreateAdvancedRuleOverrideTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<EditorSequenceFrameAnimator>(), "SequenceFrameAnimator.asset");
        }
    }
}