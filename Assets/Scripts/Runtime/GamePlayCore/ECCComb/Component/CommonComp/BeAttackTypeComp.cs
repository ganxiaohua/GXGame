using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public enum BeAttackType
    {
        CanBeAttack,
        Invincible,
    }

    public struct BeAttackTypeComp : EffComponent
    {
        public BeAttackType Value;

        public void Dispose()
        {
        }
    }
}