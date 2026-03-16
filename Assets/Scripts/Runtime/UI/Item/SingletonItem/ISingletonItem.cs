using Cysharp.Threading.Tasks;

namespace GamePlay.Runtime
{
    public interface ISingletonItem
    {
        public void Hide();

        public void Destroy();
    }
}