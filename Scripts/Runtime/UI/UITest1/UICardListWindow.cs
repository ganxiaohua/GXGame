using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public class UICardListWindow : UIEntity
    {
        private UICardListWindowView UICardListWindowView;
        public override string PackName => "Card";
        public override string WindowName => "CardListWindow";
        
        protected override async UniTaskVoid Initialize()
        {
            DependentUI despen = AddComponent<DependentUI, string, string>(PackName, WindowName);
            UICardListWindowView = new UICardListWindowView();
            var succ = await despen.WaitLoad();
            if (succ)
            {
                UICardListWindowView.Link(this, despen.Window, true);
            }
            // UIManager.Instance.AddChildUI(typeof(UICardListWindow2),UINode, UICardListWindowView.n43).Forget();
        }
        
        public override void PreShow(bool isFirstShow)
        {
            
        }
        public override void Show()
        {
            UICardListWindowView.OnShow();
        }

        public override void Hide()
        {
            UICardListWindowView.DoHideAnimation();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            UICardListWindowView.Update(elapseSeconds, realElapseSeconds);
        }

        public override void Clear()
        {
            UICardListWindowView.Clear();
            base.Clear();
        }
        
    }
}