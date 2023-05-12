
using System;
using System.Collections.Generic;
using GameFrame;
namespace GXGame
{
    public static class UICardChoiceWindowDataSystem
    {
        
        [SystemBind]
        public class UICardChoiceWindowDataStartSystem : StartSystem<UICardChoiceWindowData>
        {
            protected override void Start(UICardChoiceWindowData self)
            {
                
            }
        }

        [SystemBind]
        public class UICardChoiceWindowDataClearSystem : ClearSystem<UICardChoiceWindowData>
        {
            protected override void Clear(UICardChoiceWindowData self)
            {
                
            }
        }
        


    }
}
