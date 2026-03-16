using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public static partial class ConstOperatedEffectFunc
    {
        public static bool OnOperatedEffect_Player(EffEntity owner, EffEntity target)
        {
            bool operatedSucc = false;
            operatedSucc = OperatedLand(owner, target);
            var operatedDetection = owner.GetOperatedDetectionComp().Value;
            if (!operatedSucc)
            {
                operatedSucc = OperatedEffectTypeJudge(target, operatedDetection.OperatedType) && OperatedUnit(owner, target);
            }

            if (operatedSucc)
            {
                PlayerAttrData.Instance.OperationFatigueComparison(operatedDetection.OperatedType);
            }

            return operatedSucc;
        }

        private static bool OperatedUnit(EffEntity owner, EffEntity target)
        {
            bool succ = false;
            var unit = target.GetCurBeOperated();
            if (unit == null)
                return succ;
            switch (unit.OpType)
            {
                case UnitOpType.Attack:
                    Attack(owner, target);
                    succ = true;
                    break;
                case UnitOpType.Use:
                case UnitOpType.OpenUI:
                    // case UnitOpType.Transmit:
                    Use(owner, target);
                    succ = true;
                    break;
            }

            return succ;
        }

        private static bool OperatedLand(EffEntity owner, EffEntity target)
        {
            bool succ = false;
            if (!ConstCamp.IsCropLand(target))
                return succ;
            var operatedDetection = owner.GetOperatedDetectionComp().Value;
            var accumulateCropIndex = owner.GetAccumulateCropIndexWithHave();
            var collisionDetection = owner.GetCollisionDetectionDataComp().Value;
            switch (operatedDetection.OperatedType)
            {
                case OperatedType.播种:
                {
                    var cropLandView = (CropLandView) target.GetView().GetData();
                    succ = cropLandView.areaCropLand.Sowing(collisionDetection.Pos, operatedDetection.Itemid);
                    break;
                }
                case OperatedType.锄:
                {
                    var cropLandView = (CropLandView) target.GetView().GetData();
                    succ = !accumulateCropIndex.have ? cropLandView.areaCropLand.Reclamation(collisionDetection.Pos) : cropLandView.areaCropLand.Reclamation(accumulateCropIndex.data.GetData());

                    break;
                }
                case OperatedType.洒水:
                {
                    var cropLandView = (CropLandView) target.GetView().GetData();
                    succ = !accumulateCropIndex.have ? cropLandView.areaCropLand.Watering(collisionDetection.Pos) : cropLandView.areaCropLand.Watering(accumulateCropIndex.data.GetData());
                    break;
                }
            }

            if (accumulateCropIndex.have)
            {
                owner.RemoveComponent(ComponentsID<AccumulateCropIndex>.TID);
            }

            return succ;
        }

        /// <summary>
        /// 可以被攻击物操作
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="target"></param>
        /// <param name="operateTypedComp"></param>
        private static void Attack(EffEntity Owner, EffEntity target)
        {
            var beAttackType = target.GetBeAttackTypeCompWithHave();
            if (!beAttackType.have || beAttackType.data.Value == BeAttackType.Invincible)
                return;
            // BeAttackBuffComp beAttackBuffComp = default;
            if (!target.HasComponent<BeAttackBuffComp>())
                target.AddBeAttackBuffCompExternal(64);
            var buff = target.GetBeAttackBuffComp().GetList();
            buff.Add(new BeAttackBuff()
            {
                    Attack = Owner.GetAttrComp().GetData().GetAtk(),
                    Dir = target.GetView().GetData().Position - Owner.GetView().GetData().Position,
                    Strength = 1,
                    Attacker = Owner
            });
        }


        /// <summary>
        /// 可使用物的操作
        /// </summary>
        /// <param name="Owner"></param>
        /// <param name="target"></param>
        /// <param name="operateTypedComp"></param>
        private static void Use(EffEntity Owner, EffEntity target)
        {
            //如果被使用过了就跳过
            if (!target.HasComponent<BeUseBuffComp>())
            {
                target.AddBeUseBuffComp();
            }
        }
    }
}