using GameFrame;

public class AutoBindEvent
{
    public void AddSystem()
    {
        var  eventData =  EventData.Instance;
        
        eventData.AddSourceDic(typeof(GXGame.UICardListWindow2),typeof(GXGame.ITestEvent));
    }
}
