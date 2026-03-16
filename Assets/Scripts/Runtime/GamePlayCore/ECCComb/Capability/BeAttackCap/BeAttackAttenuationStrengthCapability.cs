using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class BeAttackAttenuationStrengthCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.BeAttackPower;

        public override bool ShouldActivate()
        {
            return Owner.HasComponent<MoveDirectionExPowerComp>();
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent<MoveDirectionExPowerComp>();
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var power = Owner.GetMoveDirectionExPowerComp().Value;
            var dir = power.Strength;
            power.Strength -= power.Strength.normalized * (delatTime * power.AttenuationStrength);
            Owner.SetMoveDirectionExPowerComp(power);
            if (Vector3.Dot(dir, power.Strength) < 0)
            {
                Owner.RemoveComponent(ComponentsID<MoveDirectionExPowerComp>.TID);
            }
        }
    }
}