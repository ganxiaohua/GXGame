using System;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;

namespace GXGame
{
    public class BaseView : IDisposable
    {
        protected EffEntity BindEntity;
        protected GameObjectView GameObjectView;

        public virtual void Init(EffEntity effEntity, GameObjectView gameObjectView)
        {
            GameObjectView = gameObjectView;
            BindEntity = effEntity;
            WaitLoadOver().Forget();
        }

        protected virtual async UniTask WaitLoadOver()
        {
            await GameObjectView.WaitLoadOver();
        }


        public virtual void Dispose()
        {
            BindEntity = null;
            GameObjectView = null;
        }
    }
}