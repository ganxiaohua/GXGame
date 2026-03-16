using GameFrame.Runtime;
using UnityEngine.Playables;

namespace GamePlay.Runtime
{
    public class AnimationBehaviour : PlayableBehaviour
    {
        public int AnimationHashIndex;

        public EffEntity EffEntity;
        //
        // public override void ProcessFrame(Playable playable, FrameData info, object playerData)
        // {
        //     if (playerData == null)
        //         return;
        //     // EffEntity ??= (ViewEffBindEnitiy) playerData;
        //     EffEntity.AddOrSetForceAnimationComp(AnimationHashIndex);
        // }

        public override void OnGraphStart(Playable playable)
        {
        }

        /// <summary>
        ///   <para>This function is called when the PlayableGraph that owns this PlayableBehaviour stops.</para>
        /// </summary>
        /// <param name="playable">The Playable that owns the current PlayableBehaviour.</param>
        public override void OnGraphStop(Playable playable)
        {
        }


        public override void OnBehaviourPlay(Playable playable, FrameData info)
        {
            // if (EffEntity != null)
            //     EffEntity.AddOrSetForceAnimationComp(AnimationHashIndex);
        }

        public override void OnBehaviourPause(Playable playable, FrameData info)
        {
            if (EffEntity != null)
                EffEntity.RemoveComponent(ComponentsID<ForceAnimationComp>.TID);
        }
    }
}