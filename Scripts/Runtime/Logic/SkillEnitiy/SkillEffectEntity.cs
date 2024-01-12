using GameFrame;

namespace GXGame
{
    public class SkillEffectEntity : ECSEntity,IStartSystem
    {
        public void Start()
        {
            Initialize();
        }
    }
}