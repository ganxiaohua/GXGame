using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public struct TurnDirectionComp : EffComponent
    {
        public Vector3 Value;

        public void Dispose()
        {
        }
    }

    public struct TurnDirectionSpeedComp : EffComponent
    {
        public float Value;

        public void Dispose()
        {
        }
    }
}