using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public struct BeAttackBuff
    {
        public Vector3 Dir;
        public IEntity Attacker;

        /// <summary>
        /// 这个数值大于被攻击者的某个稳定值就能被击退
        /// </summary>
        public float Strength;

        /// <summary>
        /// 攻击力
        /// </summary>
        public int Attack;
    }

    /// <summary>
    /// 被攻击buff 施加在被攻击者身上
    /// </summary>
    public struct BeAttackBuffComp : EffComponent
    {
        private int BeAttackBuffId;

        public void Init(int maxCount = 64)
        {
            BeAttackBuffId = ListDatas<BeAttackBuff>.Instance.AddArrayDatas(maxCount);
        }

        public List<BeAttackBuff> GetList()
        {
            return ListDatas<BeAttackBuff>.Instance.GetArrayDatas(BeAttackBuffId);
        }

        public void Dispose()
        {
            ListDatas<BeAttackBuff>.Instance.RemoveDatas(BeAttackBuffId);
        }
    }
}