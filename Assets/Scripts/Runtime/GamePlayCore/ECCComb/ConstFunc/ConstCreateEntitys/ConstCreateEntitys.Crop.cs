using System;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class ConstCreateEntitys
    {
        public static EffEntity CreateCropWithItemId(ECCWorld world, int landId, int cellIndex, Vector3 pos, int itemId)
        {
            if (!LandData.Instance.IsFarm(landId, cellIndex) || LandData.Instance.IsCrop(landId, cellIndex)) return null;
            var item = Tables.Instance.ItemTable.GetOrDefault(itemId);
            if (item.Unit == null)
            {
                return null;
            }

            return CreateCropWithUnitId(world, landId, cellIndex, pos, item.Unit.Value);
        }

        public static EffEntity CreateCropWithUnitId(ECCWorld world, int landId, int cellIndex, Vector3 pos, int unitId)
        {
            if (LandData.Instance.IsCrop(landId, cellIndex)) return null;
            return CreateCropWithUnitIdDoDetect(world, landId, cellIndex, pos, unitId);
        }

        public static EffEntity CreateCropWithUnitIdDoDetect(ECCWorld world, int landId, int cellIndex, Vector3 pos, int unitId)
        {
            if (!LandData.Instance.IsFarm(landId, cellIndex)) return null;
            var child = world.AddChild();
            var unit = Tables.Instance.UnitTable.GetOrDefault(unitId);
            child.Name = unit.Model_Ref.Path;
            child.AddAssetPathCompExternal(unit.Model_Ref.Path);
            var view = View.Create(typeof(LogicView), child, Main.GameObjectLayer);
            view.Position = pos;
            child.AddViewExternal(view);
            world.BindCapability<ViewCapability>(child);
            child.AddUnitDataCompExternal(new UnitData() {UnitItem = unit, Camp = CampType.Crop});
            ActionComp(unit, child, world, ConstDie.Crop, ConstBeOperated.BeUse_CropPickRetain, null);
            return child;
        }
    }
}