using Cysharp.Threading.Tasks;
using Eden.Gameplay.Runtime;
using GameFrame;

namespace GXGame
{
    public class UICardListWindow2 : UIEntity, ITestEvent
    {
        private UICardListWindowView2 UICardListWindowView;
        public override string PackName => "Card";
        public override string WindowName => "CardListWindow2";
        

        protected override async UniTaskVoid Initialize()
        {
            UICardListWindowView = new UICardListWindowView2();
            DependentUI despen = AddComponent<DependentUI, string, string>(PackName, WindowName);
            var succ = await despen.WaitLoad();
            if (succ)
            {
                UICardListWindowView.Link(this, despen.Window, false);
            }
            EventSend.Instance.FireTestEvent();
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
            UICardListWindowView.OnHide();
        }

        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            UICardListWindowView.Update(elapseSeconds,realElapseSeconds);
        }
        
        public  override void Clear()
        {
            UICardListWindowView.Clear();
            base.Clear();
        }

        public void Test()
        {
            Debugger.Log("接收到消息");
        }
    }
}