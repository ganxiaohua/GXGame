using System;
using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public static class UICardListWindowSystem
    {
        [SystemBind]
        public class UICardListWindowStartSystem : StartSystem<UICardListWindow>
        {
            protected override void Start(UICardListWindow self)
            {
                self.UICardListWindowView = new UICardListWindowView();
                self.UICardListWindowView.Link(self);
                List<string> temp = new List<string>();
                temp.Add("Card");
                self.AddComponent<DependentUIResources, List<string>>(temp);
            }
        }

        [SystemBind]
        public class UICardListWindowPreShowSystem : PreShowSystem<UICardListWindow>
        {
            protected override void PreShow(UICardListWindow self, bool isFirstShow)
            {
                if (self.HasComponent<UICardListWindowData>())
                {
                    self.RemoveComponent(typeof(UICardListWindowData));   
                }
                self.AddComponent<UICardListWindowData>();
                //这里可以处理通讯和其他资源加载,在完成之后在show这样可以保证不会出现闪烁出现//例子UI等待:加入self.AddComponent<WaitComponent,Type>(typeof(IUIWait));//当需求完成的时候RemoveComponent<WaitComponent>
            }
        }

        [SystemBind]
        public class UICardListWindowShowSystem : ShowSystem<UICardListWindow>
        {
            protected override void Show(UICardListWindow self)
            {
                self.UICardListWindowView.Show();
            }
        }

        [SystemBind]
        public class UICardListWindowHideSystem : HideSystem<UICardListWindow>
        {
            protected override void Hide(UICardListWindow self)
            {
                self.UICardListWindowView.Hide();
            }
        }

        [SystemBind]
        public class UICardListWindowUpdateSystem : UpdateSystem<UICardListWindow>
        {
            protected override void Update(UICardListWindow self, float elapseSeconds, float realElapseSeconds)
            {
            }
        }

        [SystemBind]
        public class UICardListWindowClearSystem : ClearSystem<UICardListWindow>
        {
            protected override void Clear(UICardListWindow self)
            {
                self.UICardListWindowView.Clear();
            }
        }

        public static void OpenCardList2(this UICardListWindow cardListWindow)
        {
            UIManager.Instance.OpenUI(typeof(UICardListWindow2));
        }
    }
}