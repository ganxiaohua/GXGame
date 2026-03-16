using System.Collections.Generic;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public struct AccumulateCropIndex : EffComponent
    {
        private int valueIndex;

        public void Init(int size = 64)
        {
            valueIndex = ListDatas<int>.Instance.AddArrayDatas(size);
        }

        public List<int> GetData()
        {
            return ListDatas<int>.Instance.GetArrayDatas(valueIndex);
        }

        public void Dispose()
        {
            ListDatas<int>.Instance.RemoveDatas(valueIndex);
        }
    }
}