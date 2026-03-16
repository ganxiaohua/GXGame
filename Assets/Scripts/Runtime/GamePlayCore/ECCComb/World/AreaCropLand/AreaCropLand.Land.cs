using System.Collections.Generic;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class AreaCropLand
    {
        public bool Reclamation(Vector3 pos)
        {
            var index = croplandData.Data.World2Index(pos);
            if (!LandData.Instance.IsFarm(croplandId, index))
            {
                LandData.Instance.SetFram(croplandId, index);
                var view = (CropLandView) land.GetView().GetData();
                view.SetLandMesh();
                return true;
            }

            return false;
        }

        public bool Reclamation(List<int> indexs)
        {
            bool succ = false;
            for (int i = 0; i < indexs.Count; i++)
            {
                int index = indexs[i];
                if (!LandData.Instance.IsFarm(croplandId, index))
                {
                    LandData.Instance.SetFram(croplandId, index);
                    var view = (CropLandView) land.GetView().GetData();
                    view.SetLandMesh();
                    succ = true;
                }
            }

            return succ;
        }
    }
}