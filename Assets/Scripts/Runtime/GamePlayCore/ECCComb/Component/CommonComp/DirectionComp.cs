using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public struct StopMoveDirection : EffComponent
    {
        public void Dispose()
        {
        }
    }

    public struct MoveDirectionComp : EffComponent
    {
        public Vector3 Value;

        public void Dispose()
        {
        }
    }

    public struct MoveDirectionExPowerData
    {
        public Vector3 Strength;
        public float AttenuationStrength;

        public MoveDirectionExPowerData(Vector3 strength, float attenuationStrength)
        {
            Strength = strength;
            AttenuationStrength = attenuationStrength;
        }
    }

    /// <summary>
    /// 额外的力量,已经算上速度而不是一个标准向量
    /// </summary>
    public struct MoveDirectionExPowerComp : EffComponent
    {
        public MoveDirectionExPowerData Value;

        public void Dispose()
        {
        }
    }
}