using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public enum HarvestState
    {
        Jump,
        Show,
        WaitingHarvest,
    }

    public struct HarvestStateComp : EffComponent
    {
        public HarvestState Value;

        public void Dispose()
        {
        }
    }
}