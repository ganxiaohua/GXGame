using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class ViewCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.View;
        private Camera camera;
        private EffEntityView view;

        public override bool ShouldActivate()
        {
            return true;
        }

        public override bool ShouldDeactivate()
        {
            return false;
        }

        public override void OnActivated()
        {
            base.OnActivated();
            view = Owner.GetView().GetData();
            if (view.notNeedBuind)
                return;
            view.BindFromAssetAsync(Owner.GetAssetPathComp().GetData(), view.parent).Forget();
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
            Owner.RemoveComponent(ComponentsID<View>.TID);
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            if (view == null || view.State != GameObjectState.Loaded)
                return;
            view.TickActive(delatTime, realElapseSeconds);
        }

        public override void Dispose()
        {
            view = null;
            base.Dispose();
        }
    }
}