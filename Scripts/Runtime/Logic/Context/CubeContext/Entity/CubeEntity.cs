using GameFrame;

namespace GXGame
{
    public class Cube : ECSEntity,IStartSystem
    {
        public void Start()
        {
            Initialize();
        }
    }
}