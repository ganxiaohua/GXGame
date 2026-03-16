using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    // public class ViewTypeComp : EffComponent
    // {
    //     public Type Value;
    // }
    //

    public struct AssetPathComp : EffComponent
    {
        private int ValueIndex;

        public void Init(string str)
        {
            ValueIndex = ObjectDatas<string>.Instance.AddData(str);
        }

        public string GetData()
        {
            return ObjectDatas<string>.Instance.GetData(ValueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<string>.Instance.RemoveData(ValueIndex);
        }
    }
}