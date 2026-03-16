using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class MapResData
    {
        //area,chunk,house,unit
        private Dictionary<int, Dictionary<int, Dictionary<int, JumpIndexArray<MapRes>>>> houseRes;

        public void PickHouseUnit(int areaId, int chunkId, int houseId, int houseUnitCount, int houseUnitIndex, int unitId)
        {
            var succ = houseRes.TryGetValue(areaId, out var chunkIndex);
            if (!succ)
            {
                chunkIndex = new();
                houseRes.Add(areaId, chunkIndex);
            }

            succ = chunkIndex.TryGetValue(chunkId, out var houseIddex);
            if (!succ)
            {
                houseIddex = new();
                chunkIndex.Add(chunkId, houseIddex);
            }

            succ = houseIddex.TryGetValue(houseUnitIndex, out var unitList);
            if (!succ)
            {
                unitList = new();
                unitList.Init(houseUnitCount);
                houseIddex.Add(houseUnitIndex, unitList);
            }

            Assert.IsTrue(unitList[houseUnitIndex] == null, "Already have this chunkUnitIndex");
            var res = ReferencePool.Acquire<MapRes>();
            res.IsPick = true;
            res.Index = houseUnitIndex;
            res.CountdownRebirth = 3;
            unitList.Set(houseUnitIndex, res);
        }

        public bool IsHousePink(int areaId, int chunkId, int houseId, int houseUnitIndex)
        {
            var succ = houseRes.TryGetValue(areaId, out var chunkIndex);
            if (!succ)
                return false;
            succ = chunkIndex.TryGetValue(chunkId, out var houseIddex);
            if (!succ)
                return false;
            succ = houseIddex.TryGetValue(houseId, out var unitList);
            if (!succ)
                return false;
            return unitList[houseUnitIndex] != null;
        }

        private void RefreshDayHouse()
        {
            foreach (var chunkDic in houseRes)
            {
                foreach (var houseDic in chunkDic.Value)
                {
                    foreach (var items in houseDic.Value)
                    {
                        var list = items.Value;
                        foreach (var mapRes in list)
                        {
                            mapRes.CountdownRebirth = Mathf.Max(0, --mapRes.CountdownRebirth);
                            if (mapRes.CountdownRebirth == 0)
                            {
                                ReferencePool.Release(mapRes);
                                list.Remove(mapRes.Index);
                            }
                        }
                    }
                }
            }
        }
    }
}