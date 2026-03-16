using GameFrame.Runtime;
using Pathfinding;

namespace GamePlay.Runtime
{
    public class SeekerCapability : CapabilityBase
    {
        private SeekerComp seekerComp;

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<SeekerEndPointComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<SeekerEndPointComp>.TID);
        }

        public override void OnActivated()
        {
            base.OnActivated();
            var endPos = Owner.GetSeekerEndPointComp().Value;
            seekerComp = Owner.GetSeekerComp();
            var seeker = seekerComp.GetData();
            var startPos = Owner.GetView().GetData().Position;
            seeker.Seeker.StartPath(startPos, endPos, FindPath);
            seeker.State = SeekerData.StateEnum.Find;
            Owner.RemoveComponent(ComponentsID<SeekerEndPointComp>.TID);
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        private void FindPath(Path p)
        {
            if (!Owner.IsAction)
                return;
            var data = seekerComp.GetData();
            data.State = SeekerData.StateEnum.FindOver;
            Assert.IsFalse(p.error, p.errorLog);
            data.CurIndex = 0;
            data.Path = p;
            data.VectorPath = p.vectorPath;
        }
    }
}