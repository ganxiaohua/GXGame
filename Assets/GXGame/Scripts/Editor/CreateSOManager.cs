using UnityEditor;
using UnityEngine;
namespace GXGame.Editor
{
    public static class CreateSOManager
    {
        [MenuItem("Assets/Create/GXGame/2DAnimator/Animator", priority = 1)]
        static void CreateAdvancedRuleOverrideTile()
        {
            ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<EditorSequenceFrameAnimator>(), "SequenceFrameAnimator.asset");
        }
        //
        // [MenuItem("Assets/Create/GXGame/TileMapCollision", priority = 1)]
        // static void CreateTileCollisionData()
        // {
        //     ProjectWindowUtil.CreateAsset(ScriptableObject.CreateInstance<TileCollisionData>(), "TilemapCollision.asset");
        // }
    }
}