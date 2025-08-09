using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime
{
    public class SearchMonsterCapability : CapabilityBase
    {
        private Group group;


        protected override void OnInit()
        {
            Matcher matcher = Matcher.SetAll(ComponentsID<Monster>.TID);
            group = World.GetGroup(matcher);
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
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
        }


        public override void TickActive(float delatTime, float realElapseSeconds)
        {
        }

        public override void Dispose()
        {
        }
    }
}