using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 农作物跳到某一点
    /// </summary>
    public class ShowItemQualityCapability : CapabilityBase
    {
        private GameObjectProxy effect;
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.ShowItemQua;
        private float expirationTime;

        protected override void OnInit()
        {
            effect = new GameObjectProxy();
            effect.Initialize();
        }

        public override bool ShouldActivate()
        {
            return Owner.GetHarvestStateComp().Value == HarvestState.Show;
        }

        public override void OnActivated()
        {
            base.OnActivated();
            var item = Owner.GetItemComp().GetLogicData();
            effect.BindFromAssetAsync(ConstPath.EffectsQuality[(int) item.Quality], Owner.GetView().GetData().transform).Forget();
            effect.LocalPosition = Vector3.zero;
            expirationTime = Time.realtimeSinceStartup + 1.5f;
        }

        public override bool ShouldDeactivate()
        {
            return false;
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            if (Time.realtimeSinceStartup >= expirationTime)
            {
                Owner.SetHarvestStateComp(HarvestState.WaitingHarvest);
            }
        }

        public override void Dispose()
        {
            effect?.Dispose();
            base.Dispose();
        }
    }
}