using GameFrame;
using GXGame;

namespace Eden.Gameplay.Runtime
{
    public class EventSend : Singleton<EventSend>
    {
       
        public void FireTestEvent()
        {
            var allenitiy = EventData.Instance.GetEnitiy(typeof(ITestEvent));
            if (allenitiy == null)
            {
                return;
            }
            foreach (var enitiy in allenitiy)
            {
                ((ITestEvent) enitiy).Test();
            }
        }
        
    }
}
