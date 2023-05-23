
using System;
using System.Collections.Generic;
using GameFrame;
namespace GXGame
{
    public static class UICardWindowDataSystem
    {
        
        [SystemBind]
        public class UICardWindowDataStartSystem : StartSystem<UICardWindowData>
        {
            protected override void Start(UICardWindowData self)
            {
                
            }
        }

        [SystemBind]
        public class UICardWindowDataUpdateSystem : UpdateSystem<UICardWindowData>
        {
            protected override void Update(UICardWindowData self,float elapseSeconds, float realElapseSeconds)
            {
                
            }
        }

    }
}
