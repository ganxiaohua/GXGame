
using System;
using System.Collections.Generic;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public static class UICardWindowSystem
    {
        
        [SystemBind]
        public class UICardWindowStartSystem : StartSystem<UICardWindow>
        {
            protected override void Start(UICardWindow self)
            {
                                
                self.UICardWindowView = new UICardWindowView();
                self.UICardWindowView.Link(self);
                List<string> temp = new List<string>();
                temp.Add("Card/Card");
                self.AddComponent<UICardWindowData>();
                self.AddComponent<DependentUIResources, List<string>>(temp);
            }
        }

        [SystemBind]
        public class UICardWindowPreShowSystem : PreShowSystem<UICardWindow>
        {
            protected override void PreShow(UICardWindow self,bool isFirstShow)
            {
                self.AddComponent<WaitComponent,Type>(typeof(IUIWait));
                //这里可以处理通讯和其他资源加载,在完成之后在show这样可以保证不会出现闪烁出现
            }
        }
        [SystemBind]
        public class UICardWindowShowSystem : ShowSystem<UICardWindow>
        {
            protected override void Show(UICardWindow self)
            {
                self.UICardWindowView.Show();
            }
        }

        [SystemBind]
        public class UICardWindowHideSystem : HideSystem<UICardWindow>
        {
            protected override void Hide(UICardWindow self)
            {
                self.UICardWindowView.Hide();
            }
        }

        [SystemBind]
        public class UICardWindowUpdateSystem : UpdateSystem<UICardWindow>
        {
            protected override void Update(UICardWindow self,float elapseSeconds, float realElapseSeconds)
            {
                if (Input.GetKeyDown(KeyCode.J))
                {
                    self.RemoveComponent(typeof(WaitComponent));
                }
            }
        }

        [SystemBind]
        public class UICardWindowClearSystem : ClearSystem<UICardWindow>
        {
            protected override void Clear(UICardWindow self)
            {
                self.UICardWindowView.Clear();
            }
        }

    }
}
