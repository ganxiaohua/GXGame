
using GameFrame;


namespace GXGame
{
    public partial class UICardListWindow2View : UIViewBase
    {
        private UICardListWindow2 m_UICardListWindow2;
        public override void OnInit()
        {
            base.OnInit();
            m_UICardListWindow2 = (UICardListWindow2)UIBase;
        }

        public override void OnShow()
        {
            base.OnShow();
        }

        public override void OnHide()
        {
            base.OnHide();
        }
        protected override void DoShowAnimation()
        {
            base.DoShowAnimation();
        }

        protected override void DoHideAnimation()
        {
            base.DoHideAnimation();
        }

        public override void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
            base.OnUpdate(elapseSeconds,realElapseSeconds);
        }

        public override void Clear()
        {
            base.Clear();
        }
    }
}