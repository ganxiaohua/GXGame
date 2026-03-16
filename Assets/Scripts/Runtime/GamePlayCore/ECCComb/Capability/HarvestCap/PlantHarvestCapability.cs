using DG.Tweening;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 农作物跳到某一点
    /// </summary>
    public class JumpoutHarvestCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.JumpoutHarvest;

        protected override void OnInit()
        {
        }

        public override bool ShouldActivate()
        {
            return Owner.GetHarvestStateComp().Value == HarvestState.Jump;
        }

        public override void OnActivated()
        {
            base.OnActivated();
            var view = Owner.GetView().GetData();
            var worldPos = view.Position;
            var targetPos = worldPos + new Vector3(Mathf.Cos(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad), 0,
                    Mathf.Sin(UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad)) * UnityEngine.Random.Range(1.5f, 2);
            view.transform.DOJump(targetPos, 2, 2, 0.3f).SetEase(Ease.InOutCirc).OnComplete(() => { Owner.SetHarvestStateComp(HarvestState.Show); })
                    .SetTarget(this);
        }

        public override bool ShouldDeactivate()
        {
            return false;
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
        }

        public override void Dispose()
        {
            base.Dispose();
            DOTween.Kill(this);
        }
    }
}