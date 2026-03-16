using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public struct ObjectComp : EffComponent
    {
        private int valueIndex;

        public void Init(object data)
        {
            valueIndex = ObjectDatas<object>.Instance.AddData(data);
        }

        public object GetData()
        {
            return ObjectDatas<object>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<object>.Instance.RemoveData(valueIndex);
        }
    }

    public struct DependEntityComp : EffComponent
    {
        private int valueIndex;

        public void Init(EffEntity data)
        {
            valueIndex = ObjectDatas<EffEntity>.Instance.AddData(data);
        }

        public EffEntity GetData()
        {
            return ObjectDatas<EffEntity>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<EffEntity>.Instance.RemoveData(valueIndex);
        }
    }
}