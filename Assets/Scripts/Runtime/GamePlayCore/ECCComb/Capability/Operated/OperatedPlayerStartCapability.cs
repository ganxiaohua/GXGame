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
            var throwComp = Owner.GetThrowCompWithHave();
            var oparated = Owner.GetEnterOparatedCompWithHave();
            return (oparated.have && oparated.data.Value.Oparated) || (throwComp.have && throwComp.data.Value);
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

            var enter = Owner.GetEnterOparatedCompWithHave();
            if (enter.have && enter.data.Value.Oparated)
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