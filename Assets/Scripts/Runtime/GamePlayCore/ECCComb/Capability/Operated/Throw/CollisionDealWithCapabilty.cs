using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class CollisionDealWithCapabilty : CapabilityBase
    {
        protected override void OnInit()
        {
            Filter(ComponentsID<OperatedObjectComp>.TID);
        }

        public override bool ShouldActivate()
        {
            var throwComp = Owner.GetOperatedObjectComp();
            return Owner.HasComponent<OperatedObjectComp>();
        }

        public override bool ShouldDeactivate()
        {
            return true;
        }

        public override void OnActivated()
        {
            base.OnActivated();
            var oo = Owner.GetOperatedObjectComp();
            if (oo.GetData().Count != 0)
                Owner.AddComponentNoGet<DestroyComp>();
        }
    }
}