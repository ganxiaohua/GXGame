using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 被攻击的行为
    /// </summary>
    public class BeAttackBehaviorCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.BeAttackBehavior;
        private float curTime;

        private float allTime;

        protected override void OnInit()
        {
            Filter(ComponentsID<BeAttackBuffComp>.TID);
        }

        public override bool ShouldActivate()
        {
            return Owner.HasComponent<BeAttackBuffComp>() && !Owner.HasComponent<DieComp>();
        }


        public override void OnActivated()
        {
            base.OnActivated();
            Owner.AddStopMoveDirection();
            allTime = Owner.GetBeAttackFuncComp().GetData().Start.Invoke(Owner, World, this);
            // Owner.GetBeAttackTypeComp().Value = BeAttackType.Invincible;
            Owner.SetBeAttackTypeComp(BeAttackType.Invincible);
            curTime = 0;
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent<BeAttackBuffComp>();
        }

        public override void OnDeactivated()
        {
            Owner.RemoveComponent(ComponentsID<GamePlay.Runtime.StopMoveDirection>.TID);
            base.OnDeactivated();
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            curTime += delatTime;
            if (curTime >= allTime)
            {
                Owner.SetBeAttackTypeComp(BeAttackType.CanBeAttack);
                Owner.GetBeAttackBuffComp().GetList().Clear();
                Owner.RemoveComponent(ComponentsID<BeAttackBuffComp>.TID);
                Owner.GetBeAttackFuncComp().GetData().End?.Invoke(Owner, World, this);
            }
        }
    }
}