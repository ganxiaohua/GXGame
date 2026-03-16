using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class OperatedStart_MonsterCountDownCapbility : CapabilityBase
    {
        private AnimationItem curAnimationItem;
        private VirtualCollision vc;
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.OperatedCountdownMonster;
        private float curTime;

        private float allTime;
        private bool Operated = false;

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<OperatedCountdownComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<OperatedCountdownComp>.TID) || Owner.HasComponent(ComponentsID<BeAttackBuffComp>.TID);
        }

        public override void OnActivated()
        {
            base.OnActivated();
            Owner.GetCapabilityComponent().Block(CapabilityTags.Tag_Behavior, this);
            var beingOperatedCountdownComp = Owner.GetOperatedCountdownComp();
            curAnimationItem = Tables.Instance.AnimationTable.GetOrDefault(beingOperatedCountdownComp.Value);
            allTime = curAnimationItem.BeforeTime + curAnimationItem.ExecutionTime + curAnimationItem.AfterTime;
            curTime = 0;
            Operated = false;
            Owner.AddOrSetOperatedEffectFuncCompExternal(ConstOperatedEffectFunc.OnOperatedEffect_Monster);
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
            Owner.GetCapabilityComponent().UnBlock(CapabilityTags.Tag_Behavior, this);
            Owner.RemoveComponent(ComponentsID<OperatedCountdownComp>.TID);
            Operated = false;
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            curTime += delatTime;
            if (curTime >= allTime)
            {
                OnDeactivated();
            }

            if (curTime >= curAnimationItem.BeforeTime)
            {
                if (!Operated)
                {
                    Operated = true;
                    SendOperated();
                }
            }
        }

        /// <summary>
        /// 自身动作
        /// </summary>
        private void SendOperated()
        {
            CalculateVirtualCollision();
        }

        private void CalculateVirtualCollision()
        {
            OperatedType type = Owner.GetUnit().OperatedItemType;
            var logicData = Owner.GetColliderLogicComp().GetLogicData();
            var monster = Owner.GetUnit().BehaviorTable_Ref;
            var view = Owner.GetView().GetData();
            vc = new VirtualCollision();
            vc.Rot = view.Rotation.normalized;
            vc.Layer = ConstLayer.OperatedLayer;
            if (logicData.Type == LogicData.ColliderEnum.BoxCollider)
            {
                vc.Pos = view.Position + vc.Rot * Vector3.forward * (logicData.Size.z / 2 + monster.BattleBehavior_Ref.AtkDistance / 2);
                vc.Size = new Vector3(logicData.Size.x / 2, logicData.Size.y, monster.BattleBehavior_Ref.AtkDistance);
            }
            else if (logicData.Type == LogicData.ColliderEnum.CapsuleCollider)
            {
                vc.Pos = view.Position + logicData.Center + vc.Rot * Vector3.forward * (logicData.Radius + monster.BattleBehavior_Ref.AtkDistance / 2);
                vc.Size = new Vector3(logicData.Radius, logicData.Height / 2, monster.BattleBehavior_Ref.AtkDistance);
            }

            OperatedDetectionData operatedDetectionData = new OperatedDetectionData();
            operatedDetectionData.OperatedType = type;
            operatedDetectionData.OperatorCount = 1;
            operatedDetectionData.Itemid = 0;
            Owner.AddOrSetOperatedDetectionComp(operatedDetectionData);
            Owner.AddOrSetCollisionDetectionDataComp(vc);
        }
    }
}