using System.Collections.Generic;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 这是碰撞检测的最后一部
    /// </summary>
    public class OperatedExecuteCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.OperatedExecute;

        protected override void OnInit()
        {
            TagList = new List<int>();
            TagList.Add(CapabilityTags.Tag_OpExcute);
            Filter(ComponentsID<OperatedObjectComp>.TID);
        }

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<OperatedObjectComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<OperatedObjectComp>.TID);
        }

        public override void OnActivated()
        {
            base.OnActivated();
            对碰撞物进行反馈();
        }

        /// <summary>
        /// 根据使用的道具和触碰到的东西进行反馈
        /// </summary>
        private void 对碰撞物进行反馈()
        {
            int succNum = 0;
            var operatedFuncComp = Owner.GetOperatedEffectFuncComp();
            var operatedDetectionComp = Owner.GetOperatedDetectionComp().Value;
            Assert.IsNotNull(operatedFuncComp, $"{Owner.Name} 没有碰撞回馈函数");
            if (!Owner.HasComponent<OperatedObjectComp>())
                return;
            var targets = Owner.GetOperatedObjectComp().GetData();
            if (targets.Count == 0)
                return;
            for (int i = targets.Count - 1; i >= 0; i--)
            {
                var target = targets[i];
                if (!target.IsAction)
                {
                    targets.RemoveAtSwapBack(i);
                    continue;
                }

                var succ = operatedFuncComp.GetData()(Owner, target);
                if (succ)
                    succNum++;
                if (succNum == operatedDetectionComp.OperatorCount)
                {
                    return;
                }
            }
        }
    }
}