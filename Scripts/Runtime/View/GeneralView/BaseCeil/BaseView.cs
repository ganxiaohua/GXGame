using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public class BaseView : IReference
    {
        protected ECSEntity BindEntity;
        protected GameObjectView GameObjectView;

        public virtual void Init(ECSEntity ecsEntity, GameObjectView gameObjectView)
        {
            GameObjectView = gameObjectView;
            BindEntity = ecsEntity;
            WaitLoadOver().Forget();
        }

        protected virtual async UniTask WaitLoadOver()
        {
            await GameObjectView.WaitLoadOver();
        }


        public virtual void Clear()
        {
            BindEntity = null;
            GameObjectView = null;
        }
    }
}