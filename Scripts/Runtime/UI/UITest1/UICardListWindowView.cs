using System;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class UICardListWindowView : UIViewBase
    {
        private UICardListWindow m_UICardListWindow;

        public override void OnInit()
        {
            base.OnInit();
            n43.onClick.Add(() =>
            {
                UIManager.Instance.OpenUI(typeof(UICardListWindow2));
            });
        }

        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }
        
        protected override void OnHideLater()
        {
            base.OnHideLater();
        }

        protected override void DoShowAnimation()
        {
            base.DoShowAnimation();
        }

        protected override void DoHideAnimation()
        {
            base.DoHideAnimation();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds, realElapseSeconds);
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}