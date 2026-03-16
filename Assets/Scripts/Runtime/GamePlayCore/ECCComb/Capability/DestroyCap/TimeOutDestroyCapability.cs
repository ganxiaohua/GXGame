using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class TimeOutDestroyCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.BeBavior;

        public override bool ShouldActivate()
        {
            // return true;
            return Owner.HasComponent(ComponentsID<TimeOutDestroyComp>.TID);
        }


        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<TimeOutDestroyComp>.TID);
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var time = Owner.GetTimeOutDestroyComp().Value;
            if (Time.realtimeSinceStartup >= time)
            {
                Owner.AddDestroyComp();
            }
        }
    }
}