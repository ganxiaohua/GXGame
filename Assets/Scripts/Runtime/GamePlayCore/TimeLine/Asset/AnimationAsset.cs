using GameFrame.Runtime;
using UnityEngine;
using UnityEngine.Playables;

namespace GamePlay.Runtime
{
    public class AnimationAsset : PlayableAsset
    {
        public int AnimationHashIndex;

        public EffEntity EffEntity;

        public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
        {
            var playable = ScriptPlayable<AnimationBehaviour>.Create(graph);
            var behaviour = playable.GetBehaviour();
            behaviour.AnimationHashIndex = AnimationHashIndex;
            behaviour.EffEntity = EffEntity;
            return playable;
        }
    }
}