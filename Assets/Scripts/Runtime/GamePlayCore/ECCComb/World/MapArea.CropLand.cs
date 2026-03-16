using System;
using System.Collections.Generic;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public partial class MapArea
    {
        // private HashSet<int> landCropData;

        private class LandCropData : IDisposable
        {
            public int Counter;
            public AreaCropLand AreaCropLand;

            public void Dispose()
            {
                Counter = 0;
                AreaCropLand = null;
            }
        }


        private Dictionary<int, LandCropData> cropLands;

        private void InitLandCrop()
        {
            cropLands = new Dictionary<int, LandCropData>();
        }

        private void DisaposableCropLand()
        {
            foreach (var value in cropLands)
            {
                ReferencePool.Release(value.Value.AreaCropLand);
                ReferencePool.Release(value.Value);
            }

            cropLands.Clear();
        }

        public void AddCropland(int id)
        {
            if (cropLands.TryGetValue(id, out var value))
            {
                value.Counter++;
                return;
            }

            var areaCropLand = ReferencePool.Acquire<AreaCropLand>();
            areaCropLand.Init(this, AreaId, id, defaultAsset);
            var landCropData = ReferencePool.Acquire<LandCropData>();
            landCropData.Counter++;
            landCropData.AreaCropLand = areaCropLand;
            cropLands.Add(id, landCropData);
        }

        public void SubCropland(int id)
        {
            if (!cropLands.TryGetValue(id, out var value))
            {
                return;
            }

            if (--value.Counter == 0)
            {
                ReferencePool.Release(value.AreaCropLand);
                ReferencePool.Release(value);
                cropLands.Remove(id);
            }
        }

        public void DaySettlement()
        {
            foreach (var land in cropLands)
            {
                land.Value.AreaCropLand.DaySettlement();
            }
        }


        public void RefreshClopDate()
        {
            foreach (var land in cropLands)
            {
                land.Value.AreaCropLand.RefreshDate();
            }
        }
    }
}