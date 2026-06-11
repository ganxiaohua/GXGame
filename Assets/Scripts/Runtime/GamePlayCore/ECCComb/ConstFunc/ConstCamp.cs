using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public static class ConstCamp
    {
        public static bool IsCropLand(EffEntity effEntity)
        {
            if (!effEntity.IsAction)
                return false;
            var haveUnitData = effEntity.TryGetUnitDataComp(out var unit);
            if (!haveUnitData) return false;
            return unit.GetData().Camp == CampType.CropLand;
        }
    }
}