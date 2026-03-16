using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class AreaCropLand
    {
        private void InitCrop()
        {
            if (croplandData == null)
                return;
            var crops = LandData.Instance.GetCropWithLand(croplandData.Data.Index);
            int landIndex = croplandData.Data.Index;
            foreach (var crop in crops)
            {
                var worldPos = croplandData.Data.IndexToWorld(crop.Index);
                var item = ConstCreateEntitys.CreateCropWithUnitIdDoDetect(mapArea.EccWorld, landIndex, crop.Index, worldPos, crop.UnitId);
                if (item != null)
                {
                    cropDic.Add(crop.Index, item);
                    item.AddDependEntityCompExternal(land);
                }
            }
        }

        public bool Sowing(Vector3 pos, int itemid)
        {
            if (croplandData == null)
                return false;
            int landIndex = croplandData.Data.Index;
            var cellPos = croplandData.Data.WorldToCell(pos);
            var worldPos = croplandData.Data.CellCenterToWolrd(cellPos);
            var cellIndex = croplandData.Data.Cell2Index(cellPos);
            if (!LandData.Instance.IsFarm(landIndex, cellIndex))
                return false;
            if (LandData.Instance.HasCrop(landIndex, cellIndex))
                return false;
            Sowing(landIndex, cellIndex, worldPos, itemid);
            return true;
        }

        private void Sowing(int landIndex, int cellIndex, Vector3 worldPos, int itemid)
        {
            SetCrop(landIndex, cellIndex, worldPos, itemid);
            BagData.Instance.SubPocketItemWithId(itemid, 1);
        }

        private void SetCrop(int landIndex, int cellIndex, Vector3 worldPos, int itemid)
        {
            var item = ConstCreateEntitys.CreateCropWithItemId(mapArea.EccWorld, landIndex, cellIndex, worldPos, itemid);
            if (item != null)
                cropDic.Add(cellIndex, item);
            item.AddDependEntityCompExternal(land);
            LandData.Instance.AddCrop(landIndex, cellIndex, itemid);
        }

        public void RemoveCrop(Vector3 pos)
        {
            int landIndex = croplandData.Data.Index;
            var cellPos = croplandData.Data.WorldToCell(pos);
            var cellIndex = croplandData.Data.Cell2Index(cellPos);
            RemoveCrop(landIndex, cellIndex);
        }

        private void RemoveCrop(int landIndex, int index)
        {
            cropDic.Remove(index, out var croy);
            LandData.Instance.RemoveCrop(landIndex, index);
        }

        private void Grow()
        {
            int landIndex = croplandData.Data.Index;
            var array = LandData.Instance.GetCropWithLand(croplandData.Data.Index);
            foreach (var item in array)
            {
                var water = LandData.Instance.IsSprinkleWater(item.LandId, item.Index);
                if (!water)
                    continue;
                var grow = Tables.Instance.GrowthTable.GetOrDefault(item.UnitId);
                item.PersistentDay++;
                if (grow.PersistentDay != 0 && item.PersistentDay >= grow.PersistentDay)
                {
                    cropDic[item.Index].AddComponentNoGet<DestroyComp>();
                    RemoveCrop(landIndex, item.Index);
                    SetCrop(landIndex, item.Index, croplandData.Data.IndexToWorld(item.Index), grow.NextState.Value);
                }
            }
        }
    }
}