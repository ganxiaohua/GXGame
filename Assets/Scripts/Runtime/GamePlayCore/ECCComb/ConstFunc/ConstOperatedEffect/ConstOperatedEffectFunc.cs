using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    /// <summary>
    /// 对操作者产生的影响
    /// </summary>
    public static partial class ConstOperatedEffectFunc
    {
        private static bool OperatedEffectTypeJudge(EffEntity target, OperatedType operatedType)
        {
            var unit = target.GetUnit();
            if (unit == null)
            {
                return false;
            }

            var count = unit.BeOperated.Length;
            for (int i = 0; i < count; i++)
            {
                var item = unit.BeOperated[i].OperatedTypes;
                if (item == null)
                    continue;
                var b = IsCanOperate(operatedType, item);
                if (b)
                {
                    target.AddOrSetBeOperatedIndex(i);
                    return true;
                }
            }

            return false;
        }


        /// <summary>
        /// 这个目标物体的操作类型是否是可以被操作的类型
        /// </summary>
        /// <param name="ownerType"></param>
        /// <param name="targetType"></param>
        /// <returns></returns>
        private static bool IsCanOperate(OperatedType ownerType, OperatedType[] targetType)
        {
            bool canOperate = targetType == null || targetType.Length == 0;
            if (targetType.Length != 0)
            {
                foreach (var id in targetType)
                {
                    if (ownerType == id)
                    {
                        canOperate = true;
                        break;
                    }
                }
            }

            return canOperate;
        }
    }
}