
using System;
using FairyGUI;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class UICardListWindow2View : UIViewBase
    {
        private UICardListWindow2 m_UICardListWindow2;
        protected override void OnInit()
        {
            base.OnInit();
            ViewInit();
            contentPane = UIPackage.CreateObject("Card", "CardListWindow2").asCom;
            m_UICardListWindow2 = (UICardListWindow2)UIBase;
            n43.onClick.Add(m_UICardListWindow2.Back);
        }

        protected override void OnShown()
        {
            base.OnShown();
        }

        protected override void OnHide()
        {
            base.OnHide();
        }

        public override void Dispose()
        {
            base.Dispose();
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
            base.Update(elapseSeconds,realElapseSeconds);
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}