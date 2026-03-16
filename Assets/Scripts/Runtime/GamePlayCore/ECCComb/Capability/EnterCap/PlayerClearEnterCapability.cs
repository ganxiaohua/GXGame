using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    // public class PlayerClearEnterCapability : CapabilityBase
    // {
    //     public override CapabilitysUpdateMode UpdateMode { get; protected set; } = CapabilitysUpdateMode.FixedUpdate;
    //     public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.ClearEnter;
    //
    //     public override bool ShouldActivate()
    //     {
    //         return true;
    //     }
    //
    //     public override bool ShouldDeactivate()
    //     {
    //         return false;
    //     }
    //
    //     public override void TickActive(float delatTime, float realElapseSeconds)
    //     {
    //         // Owner.SetMoveDirectionComp(Vector3.zero);
    //         // Owner.SetEnterOparatedComp(new OparatedData() {Oparated = false, IsLongTouch = false});
    //         // Owner.SetRunSpeedUpComp(1);
    //         // Owner.SetMoveSpeedComp(1);
    //         // Owner.SetTurnDirectionComp(Vector3.zero);
    //     }
    // }
}