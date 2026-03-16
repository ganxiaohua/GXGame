using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class OperatedStart_AlwaysDetectionCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.AlwaysOperatedDetection;

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<ColliderLogicComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<ColliderLogicComp>.TID);
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var collider = Owner.GetColliderLogicComp().GetLogicData();
            var view = Owner.GetView().GetData();
            var pos = view.Position + collider.Center;
            var size = collider.Size;
            var rot = view.Rotation;
            Owner.AddOrSetOperatedDetectionComp(new OperatedDetectionData()
            {
                    OperatorCount = 1,
                    OperatedType = OperatedType.打击,
            });
            Owner.AddOrSetCollisionDetectionDataComp(new VirtualCollision() {Pos = pos, Rot = rot, Size = size, Layer = ConstLayer.OperatedLayer});
        }
    }
}