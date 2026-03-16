using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public interface IRefreshHp : IEvent
    {
        public void OnRefreshHp(int curHp, int MaxHp);
    }
}