using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    //玩家操作，是否蓄力。
    public class OperatedStart_PlayerCountdownCapability : CapabilityBase
    {
        private AnimationItem curAnimationItem;

        private float curTime;

        private float allTime;

        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.OperatedCountdown;
        private bool operated = false;

        private bool isLongTouch;

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<OperatedCountdownComp>.TID);
        }


        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<OperatedCountdownComp>.TID) || Owner.HasComponent(ComponentsID<BeAttackBuffComp>.TID);
        }

        public override void OnActivated()
        {
            base.OnActivated();
            var beingOperatedCountdownComp = Owner.GetOperatedCountdownComp();
            curAnimationItem = Tables.Instance.AnimationTable.GetOrDefault(beingOperatedCountdownComp.Value);
            allTime = curAnimationItem.BeforeTime + curAnimationItem.ExecutionTime + curAnimationItem.AfterTime;
            curTime = 0;
            operated = false;
            Owner.AddOrSetOperatedEffectFuncCompExternal(ConstOperatedEffectFunc.OnOperatedEffect_Player);
            var enterData = Owner.GetEnterOparatedComp().Value;
            isLongTouch = enterData.IsLongTouch;
            if (!isLongTouch)
            {
                Owner.GetCapabilityComponent().Block(CapabilityTags.Tag_KCC, this);
            }
            else
            {
                allTime = int.MaxValue;
            }
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
            Owner.GetCapabilityComponent().UnBlock(CapabilityTags.Tag_KCC, this);
            Owner.RemoveComponent(ComponentsID<OperatedCountdownComp>.TID);
        }


        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            curTime += delatTime;
            var curLongTouch = Owner.GetEnterOparatedComp().Value.IsLongTouch;
            //如果是长时间操作，或者是长按进入的能力，当前长按又取消之后
            if (curTime >= allTime || (isLongTouch && !curLongTouch))
            {
                OnDeactivated();
                return;
            }

            //执行一次操作。
            if (!isLongTouch && curTime >= curAnimationItem.BeforeTime)
            {
                if (!operated)
                {
                    Owner.GetOperatedFuncComp().GetData()(Owner, World);
                    operated = true;
                }
            }
            else if (isLongTouch && curTime >= curAnimationItem.BeforeTime)
            {
                Owner.GetOperatedFuncComp().GetData()(Owner, World);
            }
        }

        // /// <summary>
        // /// 技能
        // /// </summary>
        // private bool CreateOperated()
        // {
        //     var iteminfo = BagData.Instance.GetCurPocketInfo();
        //     if (iteminfo == null)
        //         return false;
        //     // if (iteminfo.Item.ItemType == ItemType.Weapon)
        //     // {
        //     //     ConstCreateEntitys.CreateJianQi(World, Owner);
        //     //     return true;
        //     // }
        //     return false;
        // }
    }
}