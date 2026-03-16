using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class UpdateCollisionDataCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.UpdateCollision;
        private LogicData logicData;
        private EffEntityView view;

        public override bool ShouldActivate()
        {
            if (Owner.HasComponent<View>())
                view = Owner.GetView().GetData();
            if (Owner.HasComponent<ColliderLogicComp>())
                logicData = Owner.GetColliderLogicComp().GetLogicData();
            return logicData != null;
        }

        public override bool ShouldDeactivate()
        {
            var comp = Owner.GetColliderLogicComp();
            if (!Owner.HasComponent<ColliderLogicComp>())
                logicData = null;
            return logicData == null;
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var vc = new VirtualCollision();
            vc.Rot = view.Rotation.normalized;
            if (logicData.Type == LogicData.ColliderEnum.BoxCollider)
            {
                vc.Pos = view.Position;
                vc.Size = logicData.Size;
                vc.Rot = view.Rotation;
            }
            else
            {
                Debugger.LogWarning("未完成的碰撞类型判断！");
            }

            vc.Layer = ConstLayer.AllOperatedLayer;
            Owner.AddOrSetCollisionDetectionDataComp(vc);
        }
    }
}