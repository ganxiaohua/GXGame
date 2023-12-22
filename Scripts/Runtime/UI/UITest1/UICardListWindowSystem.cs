using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
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
                Init(self).Forget();
            }

            private async UniTaskVoid Init(UICardListWindow self)
            {
                self.UICardListWindowView = new UICardListWindowView();
                DependentUI despen = self.AddComponent<DependentUI, string, string>(self.PackName, self.WindowName);
                var succ = await despen.WaitLoad();
                if (succ)
                {
                    self.UICardListWindowView.Link(self, despen.Window,true);
                }
                UIManager.Instance.AddChildUI(typeof(UICardListWindow2),despen.UINode,self.UICardListWindowView.n43).Forget();
            }
        }

        [SystemBind]
        public class UICardListWindowPreShowSystem : PreShowSystem<UICardListWindow>
        {
            protected override void PreShow(UICardListWindow self, bool isFirstShow)
            {
                //这里可以处理通讯和其他资源加载,在完成之后在show这样可以保证不会出现闪烁出现//例子UI等待:加入self.AddComponent<WaitComponent,Type>(typeof(IUIWait));//当需求完成的时候RemoveComponent<WaitComponent>
            }
        }

        [SystemBind]
        public class UICardListWindowShowSystem : ShowSystem<UICardListWindow>
        {
            protected override void Show(UICardListWindow self)
            {
                self.UICardListWindowView.OnShow();
            }
        }

        [SystemBind]
        public class UICardListWindowHideSystem : HideSystem<UICardListWindow>
        {
            protected override void Hide(UICardListWindow self)
            {
                self.UICardListWindowView.DoHideAnimation();
            }
        }

        [SystemBind]
        public class UICardListWindowUpdateSystem : UpdateSystem<UICardListWindow>
        {
            protected override void Update(UICardListWindow self, float elapseSeconds, float realElapseSeconds)
            {
                  self.UICardListWindowView.Update(elapseSeconds,realElapseSeconds);
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
    }
}