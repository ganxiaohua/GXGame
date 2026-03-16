using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public struct ItemComp : EffComponent
    {
        public int valueIndex;

        public void Init(Item data)
        {
            valueIndex = ObjectDatas<Item>.Instance.AddData(data);
        }

        public Item GetLogicData()
        {
            return ObjectDatas<Item>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<Item>.Instance.RemoveData(valueIndex);
        }
    }

    public struct ItemCountComp : EffComponent
    {
        public int Value;

        public void Dispose()
        {
        }
    }
}