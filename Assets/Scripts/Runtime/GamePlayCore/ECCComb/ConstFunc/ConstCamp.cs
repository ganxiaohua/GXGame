using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public static class ConstCamp
    {
        public static bool IsCropLand(EffEntity effEntity)
        {
            if (!effEntity.IsAction)
                return false;
            var unitData = effEntity.GetUnitDataCompWithHave();
            if (!unitData.have) return false;
            return unitData.data.GetData().Camp == CampType.CropLand;
        }
    }
}