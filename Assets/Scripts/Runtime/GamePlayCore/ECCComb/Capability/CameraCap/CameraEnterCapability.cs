using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class CameraEnterCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.CameraEnter;

        protected override void OnInit()
        {
            Owner.AddCameraLensZoomAngleComp(ConstData.Camera2FollowerTrankAngle[0]);
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
            var delta = GameInput.Instance.ChangeCameraWheel();
            if (delta == 0)
                return;
            var v = Owner.GetCameraLensZoomAngleComp().Value;
            v += delta;
            v = Mathf.Max(ConstData.Camera2FollowerTrankAngle[0], v);
            v = Mathf.Min(ConstData.Camera2FollowerTrankAngle[1], v);
            Owner.SetCameraLensZoomAngleComp(v);
        }
    }
}