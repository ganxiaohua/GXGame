using System;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using Gameplay.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GamePlay.Runtime
{
    public partial class GameInput
    {
        private float bagSelectTime = 0;

        private void MouseWheel(InputAction.CallbackContext context)
        {
            Vector2 scrollValue = context.ReadValue<Vector2>();
            if (!isCommonCtrl)
            {
                if (bagSelectTime + 0.2f > Time.realtimeSinceStartup)
                    return;
                bagSelectTime = Time.realtimeSinceStartup;
                BagData.Instance.SetCurPocketndex(BagData.Instance.CurBagIndex + (scrollValue.y > 0 ? -1 : 1));
                EventSend.Instance.FireUIEvent(GamePlay.Runtime.UIEventMsg.IBagSelectEvent, null);
            }
        }

        private void OnOpenBag(InputAction.CallbackContext context)
        {
            var panel = UISystem.Instance.FindFirstOfPanel<MainBagPanel>();
            if (panel == null || panel.PanelState != PanelState.Open)
            {
                UISystem.Instance.ShowUniquePanelAsync<MainBagPanel>().Forget();
            }
            else
            {
                UISystem.Instance.HidePanel(panel);
            }
        }
    }
}