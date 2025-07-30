using GameFrame.Runtime;
using UnityEngine;

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
            CreateMap();
            CreatePlayer();
        }

        private void CreateMap()
        {
            var child = AddChild();
            child.AddViewType(typeof(GoBaseView));
            child.AddAssetPath("Map_BaseMap");
            child.AddWorldPos(Vector3.zero);
            BindCapabilityUpdate<ViewCapability>(child);
        }

        private void CreatePlayer()
        {
            var palyer = AddChild();
            palyer.AddViewType(typeof(Go2DView));
            palyer.AddAssetPath("Player/Prefab/Player");
            BindCapabilityUpdate<ViewCapability>(palyer);
            BindCapabilityUpdate<MoveCapability>(palyer);
            BindCapabilityUpdate<AtkStartCapability>(palyer);
            BindCapabilityUpdate<AtkingCapability>(palyer);
        }
    }
}