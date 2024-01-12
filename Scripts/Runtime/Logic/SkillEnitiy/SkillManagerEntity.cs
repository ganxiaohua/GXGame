using GameFrame;

namespace GXGame
{
    public class SkillManagerEntity : ECSEntity,IStartSystem
    {
        public void Start()
        {
            Initialize();
        }
    }
}