
using System;
using FairyGUI;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public partial class UICardListWindowView : UIViewBase
    {
        private UICardListWindow m_UICardListWindow;
        protected override void OnInit()
        {
            base.OnInit();
            contentPane = UIPackage.CreateObject("Card", "CardListWindow").asCom;
            m_UICardListWindow = (UICardListWindow)UIBase;
            n43.onClick.Add(m_UICardListWindow.OpenCardList2);
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