using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class CommonMoveCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.CommonMove;

        public override bool ShouldActivate()
        {
            return Owner.GetView().GetData().State == GameObjectState.Loaded;
        }

        public override bool ShouldDeactivate()
        {
            return Owner.GetView().GetData().State != GameObjectState.Loaded;
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var directionComp = Owner.GetMoveDirectionComp();
            var speed = Owner.GetMoveSpeedComp().Value;
            var view = Owner.GetView().GetData();
            view.Position += directionComp.Value * (speed * delatTime);
        }
    }
}