
using System;
using System.Collections.Generic;
using GameFrame;
namespace GXGame
{
    public static class UICardListWindowDataSystem
    {
        
        [SystemBind]
        public class UICardListWindowDataStartSystem : StartSystem<UICardListWindowData>
        {
            protected override void Start(UICardListWindowData self)
            {
                
            }
        }

        [SystemBind]
        public class UICardListWindowDataUpdateSystem : UpdateSystem<UICardListWindowData>
        {
            protected override void Update(UICardListWindowData self,float elapseSeconds, float realElapseSeconds)
            {
                
            }
        }

    }
}
