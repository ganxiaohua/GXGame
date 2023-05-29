
using System;
using System.Collections.Generic;
using GameFrame;
namespace GXGame
{
    public static class UIHomeMainPanelDataSystem
    {
        
        [SystemBind]
        public class UIHomeMainPanelDataStartSystem : StartSystem<UIHomeMainPanelData>
        {
            protected override void Start(UIHomeMainPanelData self)
            {
                
            }
        }

        [SystemBind]
        public class UIHomeMainPanelDataUpdateSystem : UpdateSystem<UIHomeMainPanelData>
        {
            protected override void Update(UIHomeMainPanelData self,float elapseSeconds, float realElapseSeconds)
            {
                
            }
        }

    }
}
