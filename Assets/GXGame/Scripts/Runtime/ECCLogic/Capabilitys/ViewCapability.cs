using System;
using GameFrame.Runtime;

namespace GXGame.Runtime
{
    public class ViewCapability : CapabilityBase
    {
        private IEceView view;

        public override void Init(EffEntity owner, int id)
        {
            base.Init(owner, id);
        }


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
            Type type = Owner.GetViewType().Value;
            view = View.Create(type);
            view.Link(Owner);
        }

        public override void OnDeactivated()
        {
            base.OnActivated();
            view?.Dispose();
        }


        public override void TickActive(float delatTime)
        {
        }

        public override void Dispose()
        {
        }
    }
}