using System;
using System.Collections.Generic;
using GameFrame;

namespace GXGame
{
    public static class UICardListWindow2System
    {
        [SystemBind]
        public class UICardListWindow2StartSystem : StartSystem<UICardListWindow2>
        {
            protected override void Start(UICardListWindow2 self)
            {
                self.UICardListWindow2View = new UICardListWindow2View();
                self.UICardListWindow2View.Link(self);
                List<string> temp = new List<string>();
                temp.Add("TestUI/Card");
                self.AddComponent<DependentUIResources, List<string>>(temp);
            }
        }

        [SystemBind]
        public class UICardListWindow2PreShowSystem : PreShowSystem<UICardListWindow2>
        {
            protected override void PreShow(UICardListWindow2 self, bool isFirstShow)
            {
                if (self.HasComponent<UICardListWindow2Data>())
                {
                    self.RemoveComponent(typeof(UICardListWindow2Data));   
                }
                self.AddComponent<UICardListWindow2Data>();
                //这里可以处理通讯和其他资源加载,在完成之后在show这样可以保证不会出现闪烁出现//例子UI等待:加入self.AddComponent<WaitComponent,Type>(typeof(IUIWait));//当需求完成的时候RemoveComponent<WaitComponent>
            }
        }

        [SystemBind]
        public class UICardListWindow2ShowSystem : ShowSystem<UICardListWindow2>
        {
            protected override void Show(UICardListWindow2 self)
            {
                self.UICardListWindow2View.Show();
            }
        }

        [SystemBind]
        public class UICardListWindow2HideSystem : HideSystem<UICardListWindow2>
        {
            protected override void Hide(UICardListWindow2 self)
            {
                self.UICardListWindow2View.Hide();
            }
        }

        [SystemBind]
        public class UICardListWindow2UpdateSystem : UpdateSystem<UICardListWindow2>
        {
            protected override void Update(UICardListWindow2 self, float elapseSeconds, float realElapseSeconds)
            {
            }
        }

        [SystemBind]
        public class UICardListWindow2ClearSystem : ClearSystem<UICardListWindow2>
        {
            protected override void Clear(UICardListWindow2 self)
            {
                self.UICardListWindow2View.Clear();
            }
        }

        public static void Back(this UICardListWindow2 uICardListWindow2)
        {
            UIManager.Instance.Back();
        }
    }
}