using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public static class EasyGet
    {
        public static UnitItem GetUnit(this EffEntity entity)
        {
            var idComp = entity.GetUnitDataCompWithHave();
            if (!idComp.have)
                return null;
            return idComp.data.GetData().UnitItem;
        }

        public static ItemCount[] GetUnitAward(this EffEntity entity)
        {
            var curBeOperated = entity.GetCurBeOperated();
            if (curBeOperated == null || curBeOperated.ResultAward == null)
                return null;
            return curBeOperated.ResultAward.Award_Ref.Items;
        }

        public static BeOperated GetCurBeOperated(this EffEntity entity)
        {
            var unit = entity.GetUnit();
            if (unit == null)
                return null;
            var index = entity.GetBeOperatedIndex().Value;
            return unit.BeOperated[index];
        }
    }
}