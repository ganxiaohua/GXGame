using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime
{
    public class AtkingCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.Atk + 1;

        public override void Init(int id, ECCWorld world, EffEntity owner)
        {
            base.Init(id, world, owner);
        }

        public override bool ShouldActivate()
        {
            return Owner.GetAtkCountdownComp() != null;
        }

        public override bool ShouldDeactivate()
        {
            return Owner.GetAtkCountdownComp() == null;
        }

        public override void OnActivated()
        {
            Owner.GetCapabiltyComponent().Block(CapabilityTags.Tag_Move, this);
            base.OnActivated();
        }

        public override void OnDeactivated()
        {
            Owner.GetCapabiltyComponent().UnBlock(CapabilityTags.Tag_Move, this);
            base.OnDeactivated();
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var x = Owner.GetAtkCountdownComp().Value;
            x -= delatTime;
            Owner.SetAtkCountdownComp(x);
            if (x <= 0)
            {
                Owner.RemoveComponent(ComponentsID<GXGame.AtkCountdownComp>.TID);
                Owner.AddOrSetAtkOverComp(1);
            }
        }

        public override void Dispose()
        {
        }
    }
}