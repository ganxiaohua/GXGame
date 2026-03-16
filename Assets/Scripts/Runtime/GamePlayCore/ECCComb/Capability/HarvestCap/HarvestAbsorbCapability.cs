using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 被拾取
    /// </summary>
    public class HarvestAbsorbCapability : CapabilityBase
    {
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.HarvestAbsorb;
        private Group playerGroup;

        protected override void OnInit()
        {
            Matcher matcher = Matcher.SetAll(ComponentsID<PlayerComp>.TID);
            playerGroup = World.GetGroup(matcher);
            Owner.AddOrSetMoveSpeedComp(15.0f);
        }

        public override bool ShouldActivate()
        {
            return Owner.GetHarvestStateComp().Value == HarvestState.WaitingHarvest;
        }


        public override bool ShouldDeactivate()
        {
            return false;
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            var pos = Owner.GetView().GetData().Position;
            foreach (var player in playerGroup)
            {
                if (!player.IsAction || !player.HasComponent<ColliderLogicComp>())
                    return;
                var capsuleColliderHeight = player.GetColliderLogicComp().GetLogicData().Height / 2;
                var playerWorldPos = player.GetView().GetData().Position + new Vector3(0, capsuleColliderHeight, 0);
                var dis = Vector3.SqrMagnitude(playerWorldPos - pos);
                if (dis <= 2.0f * 2.0f && dis > 0.2)
                {
                    var dir = playerWorldPos - pos;
                    Owner.GetView().GetData().Position = (pos + dir.normalized * (Owner.GetMoveSpeedComp().Value * delatTime));
                }
                else if (dis <= 0.2f)
                {
                    var itemIDComp = Owner.GetItemComp();

                    var succ = BagData.Instance.AddItem(itemIDComp.GetLogicData().Id, Owner.GetItemCountComp().Value);
                    if (succ)
                        Owner.AddDestroyComp();
                }
            }
        }
    }
}