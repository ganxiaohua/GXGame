using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public enum WorldEventMsg
    {
        DaySettlement,
        RefreshDate,
    }

    public interface IWorldEvent : IEvent
    {
        public void WorldEvent(WorldEventMsg msg, object obj);
    }
}