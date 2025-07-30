using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime
{
    public class AtkingCapability : CapabilityBase
    {
        public override void Init(SHWorld world, EffEntity owner, int id)
        {
            base.Init(world, owner, id);
        }

        public override bool ShouldActivate()
        {
            return Owner.GetAtkCompCountdown() != null;
        }

        public override bool ShouldDeactivate()
        {
            return Owner.GetAtkCompCountdown() == null;
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
            var x = Owner.GetAtkCompCountdown().Value;
            x -= delatTime;
            Owner.SetAtkCompCountdown(x);
            if (x <= 0)
            {
                Owner.RemoveComponent(ComponentsID<GXGame.AtkCompCountdown>.TID);
                Owner.AddOrSetAtkOverComp(1);
            }
        }

        public override void Dispose()
        {
        }
    }
}