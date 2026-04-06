using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public enum UIEventMsg
    {
        IResizeWindow,
        IDragPocketItem,
        IBagSelectEvent,
        IRefreshBag,
        IRefreshActionValue,
        IRefreshHp,
        IShowIntroduce,
        IHideIntroduce,
    }

    public interface IUIEvent : IEvent
    {
        public void OnUIEvent(UIEventMsg msg, object obj);
    }
}