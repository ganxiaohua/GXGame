using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public static class UICardListWindowSystem2
    {
        [SystemBind]
        public class UICardListWindowStart2System : StartSystem<UICardListWindow2>
        {
            protected override void Start(UICardListWindow2 self)
            {
                Init(self).Forget();
            }

            private async UniTaskVoid Init(UICardListWindow2 self)
            {
                self.UICardListWindowView = new UICardListWindowView2();
                DependentUI despen = self.AddComponent<DependentUI, string, string>(self.PackName, self.WindowName);
                var succ = await despen.WaitLoad();
                if (succ)
                {
                    self.UICardListWindowView.Link(self, despen.Window,false);
                }
            }
        }

        [SystemBind]
        public class UICardListWindow2PreShowSystem : PreShowSystem<UICardListWindow2>
        {
            protected override void PreShow(UICardListWindow2 self, bool isFirstShow)
            {
                //这里可以处理通讯和其他资源加载,在完成之后在show这样可以保证不会出现闪烁出现//例子UI等待:加入self.AddComponent<WaitComponent,Type>(typeof(IUIWait));//当需求完成的时候RemoveComponent<WaitComponent>
            }
        }

        [SystemBind]
        public class UICardListWindow2ShowSystem : ShowSystem<UICardListWindow2>
        {
            protected override void Show(UICardListWindow2 self)
            {
                self.UICardListWindowView.OnShow();
            }
        }

        [SystemBind]
        public class UICardListWindow2HideSystem : HideSystem<UICardListWindow2>
        {
            protected override void Hide(UICardListWindow2 self)
            {
                self.UICardListWindowView.OnHide();
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
                self.UICardListWindowView.Clear();
            }
        }
    }
}