using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class PlayerEnterCapability : CapabilityBase
    {
        private int[] keyDir = new int[] {0, -1, 1};
        public override CapabilitysUpdateMode UpdateMode { get; protected set; } = CapabilitysUpdateMode.Update;
        public override int TickGroupOrder { get; protected set; } = CapabilityGroupOrder.Enter;

        protected override void OnInit()
        {
            TagList = new List<int>();
            TagList.Add(CapabilityTags.Tag_Enter);
        }


        public override bool ShouldActivate()
        {
            return true;
        }

        public override bool ShouldDeactivate()
        {
            return false;
        }

        public override void OnActivated()
        {
            base.OnActivated();
        }

        public override void OnDeactivated()
        {
            base.OnDeactivated();
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            int indexX = 0;
            int indexZ = 0;
            var action = GameInput.Instance.GetPlayerMoveDir();
            indexX = action.x switch
            {
                    < 0 => 1,
                    > 0 => 2,
                    _ => indexX
            };
            indexZ = action.z switch
            {
                    > 0 => 2,
                    < 0 => 1,
                    _ => indexZ
            };
            bool isLongTouch = this.CanLongTouch();
            bool useItem = GameInput.Instance.GetUseItem();
            OparatedData oparatedData = new OparatedData() {IsLongTouch = isLongTouch, Oparated = useItem || isLongTouch};
            if (!isLongTouch)
            {
                bool accelerate = GameInput.Instance.GetAccelerate();
                bool jump = GameInput.Instance.GetJump();
                bool stopRot = GameInput.Instance.GetStopRot();
                var norDir = new Vector3(keyDir[indexX], jump ? 1 : 0, keyDir[indexZ]);
                bool hasTl = PlayerAttrData.Instance.ChangeActiveValue(accelerate && (jump || norDir != Vector3.zero), 20);
                if (hasTl == false)
                    accelerate = false;
                float speedUp = accelerate ? ConstData.PlayerMoveDefSpeedUpMagnification : 1;
                float speed = stopRot ? ConstData.PlayerWalkMoveDefSpeed : (accelerate ? ConstData.PlayerRunMoveDefSpeed * ConstData.PlayerMoveDefSpeedUpMagnification : ConstData.PlayerRunMoveDefSpeed);
                SetData(speedUp, speed, norDir, norDir * (stopRot ? 0 : 1), oparatedData);
            }
            else
            {
                var norDir = new Vector3(keyDir[indexX], 0, keyDir[indexZ]);
                SetData(1, ConstData.PlayerWalkMoveDefSpeed, norDir, Vector3.zero, oparatedData);
            }
        }

        private void SetData(float speedUp, float moveSpeed, Vector3 dir, Vector3 turnDir, OparatedData oparatedData)
        {
            if (Owner.HasComponent<DieComp>())
            {
                dir = Vector3.zero;
                turnDir = Vector3.zero;
                oparatedData.Oparated = false;
            }

            Owner.AddOrSetEnterOparatedComp(oparatedData);
            Owner.SetRunSpeedUpComp(speedUp);
            Owner.SetMoveSpeedComp(moveSpeed);
            Owner.AddOrSetMoveDirectionComp(dir);
            Owner.SetTurnDirectionComp(turnDir);
            Owner.AddOrSetThrowComp(GameInput.Instance.GetThrow());
        }


        private bool CanLongTouch()
        {
            var itemInfo = BagData.Instance.GetCurPocketInfo();
            if (itemInfo != null && itemInfo.Item != null && itemInfo.Item.AccumulatePower != null)
            {
                return GameInput.Instance.GetUseLongItem();
            }

            return false;
        }

        public override void Dispose()
        {
            base.Dispose();
        }
    }
}