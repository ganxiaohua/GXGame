using Cysharp.Threading.Tasks;
using GameFrame;
namespace GXGame
{
    public class UICardListWindow : Entity, IStartSystem,IPreShow,IShowSystem,IHideSystem,IUpdateSystem,IClearSystem
    {
          public UICardListWindowView UICardListWindowView;
          public string PackName = "Card";
          public string WindowName = "CardListWindow";
          public void Start()
          {
              Init().Forget();
          }
          
          private async UniTaskVoid Init()
          {
              UICardListWindowView = new UICardListWindowView();
              DependentUI despen = AddComponent<DependentUI, string, string>(PackName, WindowName);
              var succ = await despen.WaitLoad();
              if (succ)
              {
                  UICardListWindowView.Link(this, despen.Window,true);
              }
              UIManager.Instance.AddChildUI(typeof(UICardListWindow2),despen.UINode,UICardListWindowView.n43).Forget();
          }

          public void Show()
          {
              UICardListWindowView.OnShow();
          }

          public void Hide()
          {
              UICardListWindowView.DoHideAnimation();
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
    }
}
