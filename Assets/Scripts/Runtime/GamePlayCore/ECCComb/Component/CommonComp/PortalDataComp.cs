using GameFrame.Runtime;
using GamePlay.Runtime.MapData;

namespace GamePlay.Runtime
{
    public struct PortalDataComp : EffComponent
    {
        private int valueIndex;

        public void Init(PortalUnit data)
        {
            valueIndex = ObjectDatas<PortalUnit>.Instance.AddData(data);
        }

        public PortalUnit GetData()
        {
            return ObjectDatas<PortalUnit>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<PortalUnit>.Instance.RemoveData(valueIndex);
        }
    }
}