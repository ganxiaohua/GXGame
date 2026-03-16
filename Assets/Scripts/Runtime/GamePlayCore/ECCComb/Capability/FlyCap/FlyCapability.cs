using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class FlyCapability : CapabilityBase
    {
        private Collider[] colliders = new Collider[8];
        private Vector3 directionBeforeDir;
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.Fly;

        public override bool ShouldActivate()
        {
            return !OnGround() && 玩家是否操作且正下方设定范围内区域无碰撞();
        }

        public override void OnActivated()
        {
            base.OnActivated();
            Owner.SetGravityAccelerationComp(Vector3.down * ConstData.FlyGravity);
            Owner.SetTurnDirectionSpeedComp(ConstData.FlyDirectionSpeed);
            Owner.SetGravityDirVectorComp(Vector3.zero);
        }


        public override bool ShouldDeactivate()
        {
            return CheckQuitFly();
        }


        public override void OnDeactivated()
        {
            base.OnDeactivated();
            Owner.SetGravityAccelerationComp(Vector3.down * ConstData.DefGravity);
            Owner.SetTurnDirectionSpeedComp(ConstData.DefDirectionSpeed);
            Owner.RemoveComponent(ComponentsID<FlyComp>.TID);
        }


        public override void TickActive(float delatTime, float realElapseSeconds)
        {
        }

        private bool OnGround()
        {
            return Owner.HasComponent<GroundCollisionComp>() && Owner.GetGroundCollisionComp().Value.OnGround;
        }


        private bool 玩家是否操作且正下方设定范围内区域无碰撞()
        {
            var oparated = Owner.GetEnterOparatedComp().Value;
            if (oparated.Oparated && !oparated.IsLongTouch && 向下发射一个碰撞体没有碰触地面为True())
            {
                Owner.AddFlyComp();
                return true;
            }

            return false;
        }

        private bool 向下发射一个碰撞体没有碰触地面为True()
        {
            if (!Owner.HasComponent<ColliderLogicComp>())
                return false;
            var colliderData = Owner.GetColliderLogicComp();
            var view = Owner.GetView().GetData();
            var collider = colliderData.GetLogicData();
            var pos = view.Position + new Vector3(0, -ConstData.FlyGroundHeight / 2, 0);
            var rot = view.Rotation;
            var size = new Vector3(collider.Radius * 2, ConstData.FlyGroundHeight / 2, collider.Radius * 2);
#if UNITY_EDITOR
            DebugSceneView.SetBox(this, pos, rot, size, Color.green, "飞行判断区域，没有碰触则可飞行");
#endif
            int overlappingCount = CollisionDetection.OverlapBoxNonAlloc(view.gameObject, colliders,
                    pos,
                    rot,
                    size,
                    int.MaxValue, 0.01f);
            return overlappingCount == 0;
        }

        private bool CheckQuitFly()
        {
            var v = Owner.GetEnterOparatedComp().Value;
            bool oparated = v.Oparated;
            if (OnGround() || oparated)
                return true;
            return false;
        }
    }
}