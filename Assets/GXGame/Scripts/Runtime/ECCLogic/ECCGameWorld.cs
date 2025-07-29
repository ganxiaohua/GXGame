using GameFrame.Runtime;

namespace GXGame.Runtime
{
    public class ECCGameWorld : SHWorld
    {
        private int estimateChild = 200;

        public override void OnInitialize(int compCount)
        {
            base.OnInitialize(compCount);
            InitCapabilitys(AllCapability.TotallCapabiltys, CapabilityTags.Tag_Count, estimateChild);
            EstimateChildsCount(estimateChild);
            CreatePlayer();
        }

        private void CreatePlayer()
        {
            var child = AddChild();
            child.AddViewType(typeof(GoBaseView));
            child.AddAssetPath("Map_BaseMap");
            BindCapabilityUpdate<ViewCapability>(child);
            BindCapabilityUpdate<WorldPosCapability>(child);
        }
    }
}