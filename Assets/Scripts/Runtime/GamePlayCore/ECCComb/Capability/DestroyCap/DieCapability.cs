using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class DieCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.Die;
        private float outTime;

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<DieComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<DieComp>.TID);
        }

        public override void OnActivated()
        {
            base.OnActivated();
            outTime = Owner.GetDieComp().Value;
            Owner.RemoveComponent(ComponentsID<BehaviorTreeComp>.TID);
            Owner.RemoveComponent(ComponentsID<ColliderLogicComp>.TID);
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
            OnAwardCreateItem();
            var curOp = Owner.GetCurBeOperated();
            var beOperatedFunc = Owner.GetBeAttackFuncComp();
            beOperatedFunc.GetData().Death?.Invoke(Owner, World);
            if (curOp.ResultType == ResultUnitOpType.Destroy)
            {
                Owner.RemoveComponent(ComponentsID<DieComp>.TID);
                Owner.AddDestroyComp();
            }
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            if (outTime <= Time.realtimeSinceStartup)
            {
                OnDeactivated();
            }
        }

        private void OnAwardCreateItem()
        {
            var unit = Owner.GetCurBeOperated();
            if (unit.ResultAward == null)
                return;
            if (unit.ResultAward.Type == ItemGetType.Jump)
            {
                foreach (var item in Owner.GetUnitAward())
                {
                    ConstCreateEntitys.CreateHarvest(World, item.Item_Ref, item.Count, Owner.GetView().GetData().Position);
                }
            }
            else
            {
                foreach (var item in Owner.GetUnitAward())
                {
                    BagData.Instance.AddItem(item.Item, item.Count);
                }
            }
        }
    }
}