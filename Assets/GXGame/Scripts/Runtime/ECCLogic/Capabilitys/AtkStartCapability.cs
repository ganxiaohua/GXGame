using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime
{
    public class AtkStartCapability : CapabilityBase
    {
        public override void Init(SHWorld world, EffEntity owner, int id)
        {
            base.Init(world, owner, id);
        }

        public override bool ShouldActivate()
        {
            return Owner.GetAtkCompCountdown() == null;
        }

        public override bool ShouldDeactivate()
        {
            return Owner.GetAtkCompCountdown() != null;
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
                Owner.AddOrSetAtkCompCountdown(3.0f);
            }
        }

        public override void Dispose()
        {
        }
    }
}