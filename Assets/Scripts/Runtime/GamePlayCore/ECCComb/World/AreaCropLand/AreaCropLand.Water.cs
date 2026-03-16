using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class AreaCropLand
    {
        private void InitWater()
        {
            var view = (CropLandView) land.GetView().GetData();
            view.SetLandWater();
        }

        public bool Watering(Vector3 pos)
        {
            var index = croplandData.Data.World2Index(pos);
            if (!LandData.Instance.IsSprinkleWater(croplandId, index) && LandData.Instance.IsFarm(croplandId, index))
            {
                LandData.Instance.SetSprinkleWater(croplandId, index, true);
                var view = (CropLandView) land.GetView().GetData();
                view.SetLandWater();
                return true;
            }

            return false;
        }

        public bool Watering(List<int> indexs)
        {
            bool succ = false;
            for (int i = 0; i < indexs.Count; i++)
            {
                int index = indexs[i];
                if (!LandData.Instance.IsSprinkleWater(croplandId, index) && LandData.Instance.IsFarm(croplandId, index))
                {
                    LandData.Instance.SetSprinkleWater(croplandId, index, true);
                    var view = (CropLandView) land.GetView().GetData();
                    view.SetLandWater();
                    succ = true;
                }
            }

            return succ;
        }
    }
}