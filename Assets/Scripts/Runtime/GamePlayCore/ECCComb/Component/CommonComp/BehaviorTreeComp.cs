namespace GameFrame.Runtime
{
    public struct BehaviorTreeComp : EffComponent
    {
        private int ValueIndex;

        public void Init(string str)
        {
            ValueIndex = ObjectDatas<string>.Instance.AddData(str);
        }

        public string GetString()
        {
            return ObjectDatas<string>.Instance.GetData(ValueIndex);
        }

        public void Set(string str)
        {
            ObjectDatas<string>.Instance.SetData(ValueIndex, str);
        }

        public void Dispose()
        {
            ObjectDatas<string>.Instance.RemoveData(ValueIndex);
        }
    }
}