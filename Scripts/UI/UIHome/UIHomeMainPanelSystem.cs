
using System;
using System.Collections.Generic;
using GameFrame;
namespace GXGame
{
    public static class UIHomeMainPanelSystem
    {
        
        [SystemBind]
        public class UIHomeMainPanelStartSystem : StartSystem<UIHomeMainPanel>
        {
            protected override void Start(UIHomeMainPanel self)
            {
                                
                self.UIHomeMainPanelView = new UIHomeMainPanelView();
                self.UIHomeMainPanelView.Link(self);
                List<string> temp = new List<string>();
                temp.Add("Home");
                self.AddComponent<UIHomeMainPanelData>();
                self.AddComponent<DependentUIResources, List<string>>(temp);
            }
        }

        [SystemBind]
        public class UIHomeMainPanelPreShowSystem : PreShowSystem<UIHomeMainPanel>
        {
            protected override void PreShow(UIHomeMainPanel self,bool isFirstShow)
            {
                //这里可以处理通讯和其他资源加载,在完成之后在show这样可以保证不会出现闪烁出现//例子UI等待:加入self.AddComponent<WaitComponent,Type>(typeof(IUIWait));//当需求完成的时候RemoveComponent<WaitComponent>
            }
        }
        [SystemBind]
        public class UIHomeMainPanelShowSystem : ShowSystem<UIHomeMainPanel>
        {
            protected override void Show(UIHomeMainPanel self)
            {
                self.UIHomeMainPanelView.Show();
            }
        }

        [SystemBind]
        public class UIHomeMainPanelHideSystem : HideSystem<UIHomeMainPanel>
        {
            protected override void Hide(UIHomeMainPanel self)
            {
                self.UIHomeMainPanelView.Hide();
            }
        }

        [SystemBind]
        public class UIHomeMainPanelUpdateSystem : UpdateSystem<UIHomeMainPanel>
        {
            protected override void Update(UIHomeMainPanel self,float elapseSeconds, float realElapseSeconds)
            {
                
            }
        }

        [SystemBind]
        public class UIHomeMainPanelClearSystem : ClearSystem<UIHomeMainPanel>
        {
            protected override void Clear(UIHomeMainPanel self)
            {
                self.UIHomeMainPanelView.Clear();
            }
        }

    }
}
