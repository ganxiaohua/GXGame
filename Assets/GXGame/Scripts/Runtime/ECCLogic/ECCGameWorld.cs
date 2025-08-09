using GameFrame.Runtime;
using UnityEngine;

namespace GXGame.Runtime
{
    public class ECCGameWorld : ECCWorld
    {
        private int estimateChild = 200;

        public override void OnInitialize(int compCount)
        {
            base.OnInitialize(compCount);
            InitCapabilitys(AllCapability.TotallCapabiltys, CapabilityTags.Tag_Count, estimateChild);
            EstimateChildsCount(estimateChild);
            CreateMap();
            CreatePlayer();
            CreateMonster();
        }

        private void CreateMap()
        {
            var map = AddChild();
            map.Name = "地图";
            map.AddViewType(typeof(GoBaseView));
            map.AddAssetPath("Map_BaseMap");
            map.AddWorldPos(Vector3.zero);
            BindCapability<ViewCapability>(map);
        }

        private void CreatePlayer()
        {
            var palyer = AddChild();
            palyer.Name = "主角";
            palyer.AddViewType(typeof(Go2DView));
            palyer.AddAssetPath("Player/Prefab/Player");
            BindCapability<ViewCapability>(palyer);
            BindCapability<MoveCapability>(palyer);
            BindCapability<AtkStartCapability>(palyer);
            BindCapability<AtkingCapability>(palyer);
            BindCapability<SearchMonsterCapability>(palyer);
        }

        private void CreateMonster()
        {
            for (int i = 0; i < 2; i++)
            {
                var monster = AddChild();
                monster.Name = "史莱姆";
                monster.AddComponent<Monster>();
                monster.AddViewType(typeof(Go2DView));
                monster.AddAssetPath("Monster_002/Prefab/Monster_002");
                monster.AddWorldPos(new Vector3(3 * i + 2, 0, 0));
                BindCapability<ViewCapability>(monster);
            }
        }
    }
}