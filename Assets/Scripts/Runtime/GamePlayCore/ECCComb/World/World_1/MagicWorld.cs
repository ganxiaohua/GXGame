using System.Diagnostics;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using GamePlay.Runtime.MapData;
using Sirenix.OdinInspector;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class MagicWorld : ECCWorld, IWorldEvent
    {
        private int estimateChild = 256;

        [ShowInInspector]
        public MapArea MapArea { get; private set; }

        public override void OnInitialize(int compCount)
        {
            base.OnInitialize(compCount);
            //预设数组大小，快速访问
            InitCapabilitys(AllCapability.TotallCapabiltys, CapabilityTags.Tag_Count, estimateChild);
            //预设实体大小快速访问
            EstimateChildsCount(estimateChild);
            //设置所有组件内存
            AllComponents.AddComponents(this);
#if UNITY_EDITOR
            this.OutputSize();
#endif
            MapArea = new MapArea();
            MapArea.OnInitialize(1, this);
            // PlayerInit();
            Load().Forget();
            // Temp
            BagData.Instance.AddItem(3, 1);
            BagData.Instance.AddItem(6, 1);
            BagData.Instance.AddItem(100, 20);
            BagData.Instance.AddItem(110, 20);
            BagData.Instance.AddItem(120, 20);
            BagData.Instance.AddItem(2, 1);
        }

        private async UniTask Load()
        {
            await MapArea.LoadArea();
            if (!IsAction) return;
            PlayerInit();
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
            if (player == null || !player.IsAction)
                return;
            MapArea.ObservationTarget(player.GetView().GetData().Position);
        }

        public async UniTask Transfer(PortalUnit portalUnit)
        {
            MapArea.Dispose();
            MapArea.OnInitialize(portalUnit.PortaTagertlID, this);
            player.GetCapabilityComponent().Block(CapabilityTags.Tag_KCC, null);
            await MapArea.LoadArea();
            player.GetCapabilityComponent().UnBlock(CapabilityTags.Tag_KCC, null);
            SetPlayerPQS(portalUnit.ProtalTargetPoint, Quaternion.identity, Vector3.one);
        }


        public override void Dispose()
        {
            MapArea?.Dispose();
            base.Dispose();
        }

        public void WorldEvent(WorldEventMsg msg, object obj)
        {
            switch (msg)
            {
                case WorldEventMsg.RefreshDate:
                    if (MapArea != null)
                        MapArea.RefreshDate();
                    break;
                case WorldEventMsg.DaySettlement:
                    if (MapArea != null)
                        MapArea.DaySettlement();
                    break;
            }
        }
    }
}