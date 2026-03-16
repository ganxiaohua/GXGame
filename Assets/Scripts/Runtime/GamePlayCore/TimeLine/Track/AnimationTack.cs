using GameFrame.Runtime;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace GamePlay.Runtime
{
    [TrackClipType(typeof(AnimationAsset))]
    [TrackBindingType(typeof(ViewEffBindEnitiy))]
    public class AnimationTack : TrackAsset
    {
        // public EffEntity EffEntity;

        protected override Playable CreatePlayable(PlayableGraph graph, GameObject gameObject, TimelineClip clip)
        {
            // PlayableDirector direct = graph.GetResolver() as PlayableDirector;
            return base.CreatePlayable(graph, gameObject, clip);
        }
    }
}