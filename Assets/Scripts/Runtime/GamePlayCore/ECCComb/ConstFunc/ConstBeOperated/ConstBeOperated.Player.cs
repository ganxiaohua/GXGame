using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public static partial class ConstBeOperated
    {
        public static float BeOperated_PlayerStart(EffEntity Owner, ECCWorld world, CapabilityBase initiator)
        {
            BeAttack_Repel(Owner);
            Owner.GetCapabilityComponent().Block(CapabilityTags.Tag_Enter, initiator);
            var valueType = ReferencePool.Acquire<ValueTypeInt>();
            valueType.Value1 = Owner.GetHPComp().Value;
            valueType.Value2 = Owner.GetAttrComp().GetData().GetHp();
            EventSend.Instance.FireUIEvent(UIEventMsg.IRefreshHp, valueType);
            ReferencePool.Release(valueType);
            //整理受击时间
            return 0.5f;
        }

        public static void BeOperated_PlayerEnd(EffEntity Owner, ECCWorld world, CapabilityBase initiator)
        {
            Owner.GetCapabilityComponent().UnBlock(CapabilityTags.Tag_Enter, initiator);
        }
    }
}