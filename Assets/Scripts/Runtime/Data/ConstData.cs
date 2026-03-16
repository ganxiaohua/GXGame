using UnityEngine;

namespace GamePlay.Runtime
{
    public static partial class ConstData
    {
        public const float PlayerRunMoveDefSpeed = 5;

        public const float PlayerWalkMoveDefSpeed = 2.5f;

        /// <summary>
        /// 高于这个速度动画是跑,低于是走
        /// </summary>
        public const float RunAnimationPoint = 3;

        /// <summary>
        /// 角色加速移动倍率
        /// </summary>
        public const float PlayerMoveDefSpeedUpMagnification = 2;

        /// <summary>
        /// 摄像机移动轨道半径
        /// </summary>
        public const float Camera2FollowerTrackRadius = 7;

        /// <summary>
        /// 摄像机移动轨道角度
        /// </summary>
        public static int[] Camera2FollowerTrankAngle = new int[2] {90, 160};


        public static short PocketSizeX = 10;
        public static short PocketSizeY = 1;


        /// <summary>
        /// 可使用的背包大小
        /// </summary>
        public static int PocketSize = PocketSizeX * PocketSizeY;

        /// <summary>
        /// 背包格子大小
        /// </summary>
        public static short BagSizeX = 10;

        public static short BagSizeY = 4;

        public static int BagSize = BagSizeX * BagSizeY;

        /// <summary>
        /// 常规重力
        /// </summary>
        public static float DefGravity = 30;

        /// <summary>
        /// 飞行重力
        /// </summary>
        public static float FlyGravity = 1;

        /// <summary>
        /// 默认转向速度
        /// </summary>
        public static float DefDirectionSpeed = 880;

        /// <summary>
        /// 飞行转向速度
        /// </summary>
        public static float FlyDirectionSpeed = 180;

        /// <summary>
        /// 可以飞行的离地高度
        /// </summary>
        public static float FlyGroundHeight = 2.5f;


        /// <summary>
        /// 传送点房子出现的位置
        /// </summary>
        public static Vector3 TransmitHousePos = new Vector3(-100, 0, 0);


        public static Color[] UIQuaColor =
        {
                Color.white,
                new Color32(81, 128, 216, 255),
                new Color32(84, 83, 203, 255),
                Color.yellow,
                Color.green,
                Color.red,
        };
    }
}