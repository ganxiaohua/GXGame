using System.Collections.Generic;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 一轮操作的最后阶段
    /// </summary>
    public class OperatedOverCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.OperatedOver;

        protected override void OnInit()
        {
            TagList = new List<int>();
            TagList.Add(CapabilityTags.Tag_OpExcute);
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
            Owner.RemoveComponent(ComponentsID<OperatedObjectComp>.TID);
            Owner.RemoveComponent(ComponentsID<OperatedDetectionComp>.TID);
            Owner.RemoveComponent(ComponentsID<CollisionDetectionDataComp>.TID);
        }
    }
}