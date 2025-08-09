using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime
{
    public class AtkStartCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.Atk;

        public override void Init(int id, ECCWorld world, EffEntity owner)
        {
            base.Init(id, world, owner);
        }

        public override bool ShouldActivate()
        {
            return Owner.GetAtkCountdownComp() == null;
        }

        public override bool ShouldDeactivate()
        {
            return Owner.GetAtkCountdownComp() != null;
        }

        public override void OnActivated()
        {
            base.OnActivated();
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            if (Input.GetKeyDown(KeyCode.J))
            {
                Owner.AddOrSetAtkStartComp(1);
                Owner.AddOrSetAtkCountdownComp(0.5f);
            }
        }

        public override void Dispose()
        {
        }
    }
}