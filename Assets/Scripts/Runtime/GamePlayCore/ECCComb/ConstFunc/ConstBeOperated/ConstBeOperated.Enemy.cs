using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class ConstBeOperated
    {
        public static float BeOperated_EnemyStart(EffEntity Owner, ECCWorld world, CapabilityBase initiator)
        {
            if (!BeAttack_Repel(Owner))
                return 0;
            Owner.GetCapabilityComponent().Block(CapabilityTags.Tag_Behavior, initiator);
            //整理受击时间
            return 0.5f;
        }

        public static void BeOperated_EnemyEnd(EffEntity Owner, ECCWorld world, CapabilityBase initiator)
        {
            Owner.GetCapabilityComponent().UnBlock(CapabilityTags.Tag_Behavior, initiator);
        }
    }
}