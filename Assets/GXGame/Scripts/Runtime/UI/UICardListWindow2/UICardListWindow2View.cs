using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public partial class UICardListWindow2View : UIViewBase
    {
        private UICardListWindow2 UICardListWindow2;

        public override void OnInitialize()
        {
            base.OnInitialize();
            UICardListWindow2 = (UICardListWindow2) UIBase;
            n43.onClick.Set(() => { UIManager.Instance.Back(); });
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