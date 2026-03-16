using UnityEngine;

namespace GamePlay.Runtime
{
    public static class AnimatorName
    {
        public static int Idle = Animator.StringToHash("Idle");
        public static int Walk = Animator.StringToHash("Walk");
        public static int Run = Animator.StringToHash("Run");
        public static int Jump_Air = Animator.StringToHash("Jump_Air");
        public static int Fly_1 = Animator.StringToHash("Fly_1");
        public static int Die = Animator.StringToHash("Die");
        public static int Damage = Animator.StringToHash("Damage");

        /// <summary>
        /// 用剑
        /// </summary>
        public static int Atk_1 = Animator.StringToHash("Atk_1");

        /// <summary>
        /// 用啥水壶
        /// </summary>
        public static int Atk_2 = Animator.StringToHash("Atk_2");

        /// <summary>
        /// 使用道具
        /// </summary>
        public static int CastspellB_wing = Animator.StringToHash("CastspellB_wing");

        public static int LieDown = Animator.StringToHash("LieDown");

        //---------------------------------------------------------------------
        public static int Fly = Animator.StringToHash("Fly");

        public static int BoxOpen = Animator.StringToHash("BoxOpen");

        public static int[] AnimaHashIndex = new int[]
        {
            Idle, //0
            Walk, //1
            Run, //2
            Jump_Air, //3
            Fly_1, //4
            Fly, //5
            Atk_1, //6
            Atk_2, //7
            CastspellB_wing, //8
            BoxOpen, //9
            Die, //10
            Damage, //11
        };
    }
}