using System;
using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class LandData : Singleton<LandData>, IGameData
    {
        private JumpIndexArray<bool>[] farm;

        /// <summary>
        ///  有没有洒水
        /// </summary>
        /// 
        private JumpIndexArray<bool>[] sprinkleWater;

        /// <summary>
        /// 植物ID
        /// </summary>
        /// <returns></returns>
        private JumpIndexArray<CropData>[] cropIds;

        public void Initialization()
        {
            farm = new JumpIndexArray<bool>[5];
            sprinkleWater = new JumpIndexArray<bool>[5];
            cropIds = new JumpIndexArray<CropData>[5];
        }

        public void ShutDown()
        {
        }

        public void AddLand(int id, int size)
        {
            Assert.IsFalse(id >= farm.Length, "LandData out");
            if (farm[id] != null)
                return;
            farm[id] = new JumpIndexArray<bool>();
            farm[id].Init(size);
            sprinkleWater[id] = new JumpIndexArray<bool>();
            sprinkleWater[id].Init(size);
            cropIds[id] = new JumpIndexArray<CropData>();
            cropIds[id].Init(size);
        }

        public bool IsFarm(int id, int index)
        {
            return farm[id][index];
        }


        public void SetFram(int id, int index)
        {
            farm[id].Set(index, true);
        }

        public JumpIndexArray<bool> GetAllFramArray(int id)
        {
            return farm[id];
        }


        public bool IsCrop(int id, int index)
        {
            return cropIds[id][index] != null;
        }

        public bool HasCrop(int id, int index)
        {
            return cropIds[id][index] != null;
        }

        public void AddCrop(int id, int index, int itemID)
        {
            Assert.IsTrue(id < cropIds.Length, $"{id} cropIds {cropIds.Length}  array out");
            CropData cropData = ReferencePool.Acquire<CropData>();
            cropData.LandId = id;
            cropData.Index = index;
            cropData.UnitId = itemID;
            cropData.Year = TimeData.Instance.CurYear;
            cropData.Moon = TimeData.Instance.CurMoon;
            cropData.Day = TimeData.Instance.CurDay;
            cropIds[id].Set(index, cropData);
        }

        public void RemoveCrop(int id, int index)
        {
            Assert.IsTrue(id < cropIds.Length, $"{id} cropIds {cropIds.Length}  array out");
            cropIds[id].Remove(index);
        }

        public JumpIndexArray<CropData> GetCropWithLand(int landId)
        {
            Assert.IsTrue(landId < cropIds.Length, $"{landId} cropIds {cropIds.Length}  array out");
            return cropIds[landId];
        }

        public void SetAllSprinkleWater(bool water)
        {
            if (!water)
            {
                foreach (var item in sprinkleWater)
                {
                    if (item == null)
                        continue;
                    item.Clear();
                }
            }
            else
            {
                for (int i = 0; i < farm.Length; i++)
                {
                    foreach (var landIndex in farm[i].IndexList)
                    {
                        sprinkleWater[i].Set(landIndex, true);
                    }
                }
            }
        }

        public bool IsSprinkleWater(int id, int index)
        {
            return sprinkleWater[id][index];
        }

        public void SetSprinkleWater(int id, int index, bool water)
        {
            sprinkleWater[id].Set(index, water);
        }

        public JumpIndexArray<bool> GetSprinkleWater(int id)
        {
            return sprinkleWater[id];
        }
    }
}