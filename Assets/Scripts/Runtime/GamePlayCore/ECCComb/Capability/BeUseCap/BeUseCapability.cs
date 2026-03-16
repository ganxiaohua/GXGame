using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class BeUseCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.BeUser;
        private UniqueTimer uniqueTimer;
        private BeOperated beOperated;

        protected override void OnInit()
        {
            Filter(ComponentsID<BeUseBuffComp>.TID);
        }

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<BeUseBuffComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<BeUseBuffComp>.TID);
        }

        public override void OnActivated()
        {
            base.OnActivated();
            beOperated = Owner.GetCurBeOperated();
            if (beOperated.BeOpAnimation_Ref != null)
            {
                var animator = Owner.GetView().GetData().gameObject.GetComponentInChildren<Animator>();
                animator.enabled = true;
                animator.Play(AnimatorName.AnimaHashIndex[beOperated.BeOpAnimation_Ref.AnimationHashIndex]);
                uniqueTimer ??= new UniqueTimer(OnResult);
                uniqueTimer.Schedule(beOperated.BeOpAnimation_Ref.BeforeTime);
            }
            else
            {
                OnResult();
            }
        }

        public override void OnDeactivated()
        {
            uniqueTimer?.ExecuteIfScheduled();
            base.OnDeactivated();
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
        }

        private void OnResult()
        {
            OnAward();
            BeUseFunc();
            OnResultType();
            BeOperatedType();
            Owner.RemoveComponent(ComponentsID<BeUseBuffComp>.TID);
        }

        private void BeOperatedType()
        {
            var type = beOperated.OpType;
            switch (type)
            {
                case UnitOpType.OpenUI:
                    ConstEasyUI.OpenUI(beOperated.ResultOpenUI);
                    break;
            }
        }

        private void BeUseFunc()
        {
            if (Owner.HasComponent<BeUseFuncComp>())
            {
                var func = Owner.GetBeUseFuncComp().GetData();
                var index = Owner.GetBeOperatedIndex().Value;
                if (func[index] != null)
                    func[index].Invoke(Owner, World);
            }
        }

        private void OnAward()
        {
            if (beOperated.ResultAward == null)
                return;
            if (beOperated.ResultAward.Type == ItemGetType.Jump)
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

        private void OnResultType()
        {
            if (beOperated.ResultType == ResultUnitOpType.Destroy)
            {
                Owner.AddComponentNoGet<DestroyComp>();
            }
            else if (beOperated.ResultType == ResultUnitOpType.SpendTheDay)
            {
                DataManager.Instance.RefreshDay();
            }
        }
    }
}