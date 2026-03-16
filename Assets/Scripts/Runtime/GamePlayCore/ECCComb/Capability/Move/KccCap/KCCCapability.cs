using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class KCCCapability : CapabilityBase
    {
        public override CapabilitysUpdateMode UpdateMode { get; protected set; } = CapabilitysUpdateMode.FixedUpdate;
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.KCC;

        private GroundCollision groundMsg;
        private PreviousGroundMsg previousGround;
        private CollisionMsg collisionMsg;

        protected override void OnInit()
        {
            TagList = new List<int>();
            TagList.Add(CapabilityTags.Tag_KCC);
            groundMsg = new GroundCollision();
            Owner.AddGroundCollisionComp(groundMsg);
            previousGround = new PreviousGroundMsg();
            Owner.AddPreviousGroundCompExternal(previousGround);
            collisionMsg = new CollisionMsg(0.01f);
            collisionMsg.MaskLayer = Owner.GetBodyCollisionLayerComp().Value;
            Owner.AddCollisionMsgComp(collisionMsg);
        }

        public override bool ShouldActivate()
        {
            return Owner.HasComponent(ComponentsID<ColliderLogicComp>.TID);
        }

        public override bool ShouldDeactivate()
        {
            return !Owner.HasComponent(ComponentsID<ColliderLogicComp>.TID);
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            CheckGroundedCapsule();
            Jump(delatTime);
            CollisionMovement(delatTime);
        }
    }
}