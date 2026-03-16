using System;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class GameInput
    {
        private bool isUseItem;
        private bool isAccumulate;

        private void InitGameCommon()
        {
            isUseItem = false;
            inputData.Common.UseItem.performed += (x) => { isUseItem = true; };
            inputData.Common.AccumulatePower.performed += (x => { isAccumulate = true; });
            inputData.Common.AccumulatePower.canceled += (x) => { isAccumulate = false; };
        }


        public float ChangeCameraWheel()
        {
            if (isCommonCtrl)
            {
                return inputData.Common.BagSelectAndCameraScroll.ReadValue<Vector2>().y;
            }

            return 0;
        }

        public Vector3 GetPlayerMoveDir()
        {
            return inputData.Common.Move.ReadValue<Vector3>();
        }

        public bool GetAccelerate()
        {
            bool isPressed = inputData.Common.Accelerate.IsPressed();
            if (isPressed)
                return true;
            return false;
        }

        public bool GetJump()
        {
            return inputData.Common.Jump.IsPressed();
        }

        public bool GetUseItem()
        {
            var b = isUseItem;
            isUseItem = false;
            return b;
        }

        public bool GetUseLongItem()
        {
            return isAccumulate;
        }

        public bool GetStopRot()
        {
            return inputData.Common.StopRot.IsPressed();
        }

        public bool GetThrow()
        {
            return inputData.Common.Throw.IsPressed();
        }
    }
}