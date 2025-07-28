using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public partial class UICardListWindowView : UIViewBase
    {
        private UICardListWindow UICardListWindow;

        public override void OnInitialize()
        {
            base.OnInitialize();
            UICardListWindow = (UICardListWindow) UIBase;
            n43.onClick.Add(() => { UIManager.Instance.OpenUI(typeof(UICardListWindow2)); });
        }

        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds, realElapseSeconds);
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}