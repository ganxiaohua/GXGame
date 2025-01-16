using GameFrame;

namespace GXGame
{
    public interface ITestEvent1 : IEvent
    {
        public void Test(string key);
    }
    
    public interface ITestEvent2 : IEvent
    {
        public void Test();
    }
}