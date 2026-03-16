using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public struct AttrComp : EffComponent
    {
        private int valueIndex;

        public void Init(AttrData data)
        {
            valueIndex = ObjectDatas<AttrData>.Instance.AddData(data);
        }

        public AttrData GetData()
        {
            return ObjectDatas<AttrData>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            var data = GetData();
            if (data == null)
                return;
            ReferencePool.Release(data);
            ObjectDatas<AttrData>.Instance.RemoveData(valueIndex);
        }

        public static AttrData GetAttrWithConfig(AttributeItem item)
        {
            var attrData = ReferencePool.Acquire<AttrData>();
            attrData.GetAttrWithConfig(item);
            return attrData;
        }
    }
}