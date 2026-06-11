using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 按下操作按钮
    /// </summary>
    public class OperatedPlayerStartCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.OperatedCapabiity;

        public override bool ShouldActivate()
        {
            var throwComp = Owner.TryGetThrowComp(out var throwData);
            var oparated = Owner.TryGetEnterOparatedComp(out var oparetedData);
            return (oparated && oparetedData.Value.Oparated) || (throwComp && throwData.Value);
        }

        public override bool ShouldDeactivate()
        {
            return true;
        }

        public override void OnActivated()
        {
            base.OnActivated();
            if (!接触地面())
            {
                return;
            }

            var enter = Owner.TryGetEnterOparatedComp(out var data);
            if (enter && data.Value.Oparated)
            {
                Owner.AddOrSetOperatedCountdownComp(BagData.Instance.GetCurAnimation());
                Owner.AddOrSetOperatedFuncCompExternal(ConstOperateFunc.PlayerUserItem);
            }
            else
            {
                Owner.AddOrSetOperatedCountdownComp(2);
                Owner.AddOrSetOperatedFuncCompExternal(ConstOperateFunc.PlayerThrowItem);
            }
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
        }


        private bool 接触地面()
        {
            var groundCollision = Owner.GetGroundCollisionComp();
            return groundCollision.Value.OnGround;
        }
    }
}