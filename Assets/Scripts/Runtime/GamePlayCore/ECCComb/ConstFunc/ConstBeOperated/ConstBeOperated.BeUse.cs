using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class ConstBeOperated
    {
        public static void BeUse_CropPickRetain(EffEntity owner, World world)
        {
            var unit = owner.GetUnit();
            var growthItem = Tables.Instance.GrowthTable.GetOrDefault(unit.Id);
            if (growthItem.AfterPickingState != null)
            {
                var dependEntity = owner.GetDependEntityComp().GetData();
                var landView = (CropLandView) dependEntity.GetView().GetData();
                var pos = owner.GetView().GetData().Position;
                owner.AddComponentNoGet<DestroyComp>();
                landView.areaCropLand.RemoveCrop(pos);
                landView.areaCropLand.Sowing(pos, growthItem.AfterPickingState.Value);
            }
        }
    }
}