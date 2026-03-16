using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public static class ConstCapabilityGroup
    {
        public struct KCCGroupData
        {
            public float MoveSpeed;
            public float TurnDirectionSpeed;
            public Vector3 GravityAcceleration;
            public float JumpSpeed;
            public float RunSpeed;

            public KCCGroupData(float moveSpeed, float turnDirectionSpeed, Vector3 gravityAcceleration, float jumpSpeed, float runSpeed)
            {
                MoveSpeed = moveSpeed;
                TurnDirectionSpeed = turnDirectionSpeed;
                GravityAcceleration = gravityAcceleration;
                JumpSpeed = jumpSpeed;
                RunSpeed = runSpeed;
            }
        }

        public static void KCCGroup(ECCWorld world, EffEntity effEntity, KCCGroupData data)
        {
            effEntity.AddMoveSpeedComp(data.MoveSpeed);
            effEntity.AddMoveDirectionComp(Vector3.zero);
            effEntity.AddTurnDirectionComp(Vector3.zero);
            effEntity.AddTurnDirectionSpeedComp(data.TurnDirectionSpeed);
            effEntity.AddGravityAccelerationComp(data.GravityAcceleration);
            effEntity.AddGravityDirVectorComp(Vector3.zero);
            effEntity.AddJumpSpeedComp(data.JumpSpeed);
            effEntity.AddRunSpeedUpComp(data.RunSpeed);
            effEntity.AddBodyCollisionLayerComp(ConstLayer.OperatedCollisionLayer);
            world.BindCapability<KCCCapability>(effEntity);
        }

        /// <summary>
        /// 玩家操作
        /// </summary>
        /// <param name="world"></param>
        /// <param name="effEntity"></param>
        public static void PlayerOperatedGroup(ECCWorld world, EffEntity effEntity)
        {
            world.BindCapability<OperatedStart_PlayerAccumulateCapability>(effEntity);
            world.BindCapability<OperatedPlayerStartCapability>(effEntity);
            world.BindCapability<OperatedStart_PlayerCountdownCapability>(effEntity);
            world.BindCapability<OperatedDetectionCapability>(effEntity);
            world.BindCapability<OperatedExecuteCapability>(effEntity);
            world.BindCapability<OperatedOverCapability>(effEntity);
        }

        /// <summary>
        /// 怪兽操作
        /// </summary>
        /// <param name="world"></param>
        /// <param name="effEntity"></param>
        public static void MonsterOperatedGroup(ECCWorld world, EffEntity effEntity)
        {
            effEntity.AddOperatedDetectionFilterComp(new DetectionFilter(effEntity.GetUnitDataComp().GetData().Camp));
            world.BindCapability<OperatedStart_MonsterCountDownCapbility>(effEntity);
            world.BindCapability<OperatedDetectionCapability>(effEntity);
            world.BindCapability<OperatedExecuteCapability>(effEntity);
            world.BindCapability<OperatedOverCapability>(effEntity);
        }


        public static void BeAttackGroup(ECCWorld world, EffEntity effEntity)
        {
            effEntity.AddBeAttackTypeComp(BeAttackType.CanBeAttack);
            world.BindCapability<BeAttackBehaviorCapability>(effEntity);
            world.BindCapability<BeAttackAttenuationStrengthCapability>(effEntity);
            world.BindCapability<DieCapability>(effEntity);
        }

        public static void BeUse(ECCWorld world, EffEntity effEntity)
        {
            world.BindCapability<BeUseCapability>(effEntity);
        }
    }
}