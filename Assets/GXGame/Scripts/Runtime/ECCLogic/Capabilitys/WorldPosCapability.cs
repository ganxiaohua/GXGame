using GameFrame.Runtime;

namespace GXGame.Runtime
{
    public class WorldPosCapability : CapabilityBase
    {
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

        public override void TickActive(float delatTime)
        {
            var pos = Owner.GetWorldPos();
        }

        public override void Dispose()
        {
        }
    }
}