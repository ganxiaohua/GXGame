using System;
using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class MapResData : Singleton<MapResData>, IGameData
    {
        public class MapRes : IDisposable
        {
            public bool IsPick;
            public int CountdownRebirth;
            public int Index;

            public void Dispose()
            {
                IsPick = false;
                CountdownRebirth = 0;
            }
        }

        public Dictionary<int, Dictionary<int, JumpIndexArray<MapRes>>> MapData;


        public void Initialization()
        {
            MapData = new();
            houseRes = new();
        }

        public void ShutDown()
        {
        }

        public void PickUnit(int areaId, int chunkId, int chunkUnitCount, int chunkUnitIndex, int unitId)
        {
            var succ = MapData.TryGetValue(areaId, out var chunkIndex);
            if (!succ)
            {
                chunkIndex = new();
                MapData.Add(areaId, chunkIndex);
            }

            succ = chunkIndex.TryGetValue(chunkId, out var unitList);
            if (!succ)
            {
                unitList = new();
                unitList.Init(chunkUnitCount);
                chunkIndex.Add(chunkId, unitList);
            }

            Assert.IsTrue(unitList[chunkUnitIndex] == null, "Already have this chunkUnitIndex");
            var res = ReferencePool.Acquire<MapRes>();
            res.IsPick = true;
            res.CountdownRebirth = 3;
            res.Index = chunkUnitIndex;
            unitList.Set(chunkUnitIndex, res);
        }

        public bool IsPink(int areaId, int chunkId, int chunkUnitIndex)
        {
            var succ = MapData.TryGetValue(areaId, out var chunkIndex);
            if (!succ)
                return false;
            if (!chunkIndex.TryGetValue(chunkId, out var datalist))
            {
                return false;
            }

            return datalist[chunkUnitIndex] != null;
        }

        public void RefreshAreaUnit()
        {
            foreach (var chunkDic in MapData)
            {
                foreach (var items in chunkDic.Value)
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

        public void RefreshDay()
        {
            RefreshAreaUnit();
            RefreshDayHouse();
        }
    }
}