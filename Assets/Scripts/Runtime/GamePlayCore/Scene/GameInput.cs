using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay.Runtime
{
    public partial class GameInput : Singleton<GameInput>
    {
        private InputData inputData;
        private bool isCommonCtrl;
        private int stateVer;

        public void InputTest()
        {
            inputData ??= new InputData();
            inputData.Common.BagSelectAndCameraScroll.started += MouseWheel;
            inputData.UI.OpenBag.started += OnOpenBag;
            inputData.Common.LinkButton1.started += OnCtrlStart;
            inputData.Common.LinkButton1.canceled += OnCtrlEnd;
            inputData.Common.Enable();
            inputData.UI.Enable();
            stateVer = 0;
            InitGameCommon();
        }

        public void SetCommonState(bool active)
        {
            if (active)
            {
                stateVer--;
            }
            else
            {
                stateVer++;
            }

            stateVer = Mathf.Max(0, stateVer);
            if (stateVer == 0)
            {
                inputData.Common.Enable();
            }
            else
            {
                inputData.Common.Disable();
            }
        }

        private void OnCtrlStart(InputAction.CallbackContext context)
        {
            isCommonCtrl = true;
        }

        private void OnCtrlEnd(InputAction.CallbackContext context)
        {
            isCommonCtrl = false;
        }
    }
}