
using System;
using System.Collections.Generic;
using GameFrame;
namespace GXGame
{
    public static class UICardChoiceWindowSystem
    {
        
        [SystemBind]
        public class UICardChoiceWindowStartSystem : StartSystem<UICardChoiceWindow>
        {
            protected override void Start(UICardChoiceWindow self)
            {
                                
                self.UICardChoiceWindowView = new UICardChoiceWindowView();
                self.UICardChoiceWindowView.Link(self);
                List<string> temp = new List<string>();
                temp.Add("Assets/GXGame/UI/Card/Card");
                self.AddComponent<UICardChoiceWindowData>();
                self.AddComponent<DependentUIResources, List<string>>(temp);
            }
        }

        [SystemBind]
        public class UICardChoiceWindowShowSystem : ShowSystem<UICardChoiceWindow>
        {
            protected override void Show(UICardChoiceWindow self)
            {
                self.UICardChoiceWindowView.Show();
            }
        }

        [SystemBind]
        public class UICardChoiceWindowHideSystem : HideSystem<UICardChoiceWindow>
        {
            protected override void Hide(UICardChoiceWindow self)
            {
                self.UICardChoiceWindowView.Hide();
            }
        }

        [SystemBind]
        public class UICardChoiceWindowUpdateSystem : UpdateSystem<UICardChoiceWindow>
        {
            protected override void Update(UICardChoiceWindow self,float elapseSeconds, float realElapseSeconds)
            {
                
            }
        }

        [SystemBind]
        public class UICardChoiceWindowClearSystem : ClearSystem<UICardChoiceWindow>
        {
            protected override void Clear(UICardChoiceWindow self)
            {
                self.UICardChoiceWindowView.Clear();
            }
        }
        


    }
}
