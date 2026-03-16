using System.Collections.Generic;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 操作对象
    /// </summary>
    public struct OperatedObjectComp : EffComponent
    {
        private int valueIndex;

        public void Init(int size = 64)
        {
            valueIndex = ListDatas<EffEntity>.Instance.AddArrayDatas(size);
        }

        public List<EffEntity> GetData()
        {
            return ListDatas<EffEntity>.Instance.GetArrayDatas(valueIndex);
        }

        public void Dispose()
        {
            ListDatas<EffEntity>.Instance.RemoveDatas(valueIndex);
        }
    }
}