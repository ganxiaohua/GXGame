using DG.Tweening;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class ConstBeOperated
    {
        /// <summary>
        /// 被攻击附加击退效果
        /// </summary>
        /// <param name="Owner"></param>
        public static bool BeAttack_Repel(EffEntity Owner)
        {
            if (Owner.HasComponent(ComponentsID<GameFrame.Runtime.DieComp>.TID))
                return false;
            var beAttackBuff = Owner.GetBeAttackBuffComp().GetList();
            var hp = Owner.GetHPComp().Value;
            Vector3 strength = Vector3.zero;
            foreach (var buff in beAttackBuff)
            {
                strength = strength + (buff.Dir * buff.Strength);
                hp -= buff.Attack;
            }

            Owner.AddOrSetMoveDirectionExPowerComp(new MoveDirectionExPowerData(strength, 2));
            hp = Mathf.Max(0, hp);
            Owner.SetHPComp(hp);
            if (hp == 0)
            {
                Owner.AddDieComp(Time.realtimeSinceStartup + 1.5f);
            }

            return true;
        }

        /// <summary>
        ///  播放一个通用的抖动动画
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="world"></param>
        /// <returns></returns>
        public static float BeAttack_Sundries(EffEntity owner, ECCWorld world, CapabilityBase initiator)
        {
            owner.GetView().GetData().BindingTarget.transform.GetChild(0).transform.DOShakePosition(0.1f);
            return BeAttack(owner, world, initiator);
        }


        /// <summary>
        /// 被攻击
        /// </summary>
        /// <param name="owner"></param>
        /// <param name="world"></param>
        /// <param name="initiator"></param>
        /// <returns></returns>
        public static float BeAttack(EffEntity owner, ECCWorld world, CapabilityBase initiator)
        {
            var beAttackBuff = owner.GetBeAttackBuffComp().GetList();
            var hp = owner.GetHPComp().Value;
            foreach (var buff in beAttackBuff)
            {
                hp -= buff.Attack;
            }

            hp = Mathf.Max(0, hp);
            owner.SetHPComp(hp);
            if (hp == 0)
            {
                owner.AddDieComp(0f);
            }

            return 0f;
        }
    }
}