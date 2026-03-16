using System;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    [Serializable]
    public class UnitData
    {
        //地图编辑器上的ID 不是编辑器上的物件可以不填
        public int MapUnitIndex;
        public UnitItem UnitItem;
        public CampType Camp;
    }

    public struct UnitDataComp : EffComponent
    {
        private int valueIndex;

        public void Init(UnitData data)
        {
            valueIndex = ObjectDatas<UnitData>.Instance.AddData(data);
        }

        public UnitData GetData()
        {
            return ObjectDatas<UnitData>.Instance.GetData(valueIndex);
        }

        public void Dispose()
        {
            ObjectDatas<UnitData>.Instance.RemoveData(valueIndex);
        }
    }
}