using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public static partial class ConstOperatedEffectFunc
    {
        public static bool OnOperatedEffect_Monster(EffEntity owner, EffEntity target)
        {
            var hastype = target.HasComponent<BeAttackTypeComp>();
            if (!hastype)
                return false;
            var beAttackType = target.GetBeAttackTypeComp();
            if (beAttackType.Value == BeAttackType.Invincible)
                return false;
            var operatedDetection = owner.GetOperatedDetectionComp().Value;
            if (!OperatedEffectTypeJudge(target, operatedDetection.OperatedType))
                return false;
            // BeAttackBuffComp buff = default;
            var hasBuff = target.HasComponent<BeAttackBuffComp>();
            if (!hasBuff)
                target.AddBeAttackBuffCompExternal(64);
            var value = target.GetBeAttackBuffComp().GetList();
            value.Add(new BeAttackBuff()
            {
                    Attack = owner.GetAttrComp().GetData().GetAtk(),
                    Dir = target.GetView().GetData().Position - owner.GetView().GetData().Position,
                    Strength = 1,
                    Attacker = owner
            });
            return true;
        }
    }
}