using GameFrame;

namespace GXGame
{
    public class SkillEntity : ECSEntity,IStartSystem
    {
        public void Start()
        {
            Initialize();
        }
    }
}