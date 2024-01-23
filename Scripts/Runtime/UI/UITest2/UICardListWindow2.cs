using Cysharp.Threading.Tasks;
using Eden.Gameplay.Runtime;
using GameFrame;

namespace GXGame
{
    public class UICardListWindow2 : Entity, IStartSystem, IPreShowSystem, IShowSystem, IHideSystem, IUpdateSystem, IClearSystem,ITestEvent
    {
        public UICardListWindowView2 UICardListWindowView;
        public string PackName = "Card";
        public string WindowName = "CardListWindow2";

        public void Start()
        {
            Init().Forget();
        }

        private async UniTaskVoid Init()
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

        public void PreShow(bool isFirstShow)
        {
            
        }

        public void Show()
        {
            UICardListWindowView.OnShow();
        }


        public void Hide()
        {
            UICardListWindowView.OnHide();
        }

        public void Update(float elapseSeconds, float realElapseSeconds)
        {
            UICardListWindowView.Update(elapseSeconds,realElapseSeconds);
        }
        
        public override void Clear()
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