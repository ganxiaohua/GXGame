using Common.Runtime;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public abstract class GameObjectView : IEceView, IWolrdPosition, IWorldRotate, ILocalScale,ILocalPosition,ILocalRotate
    {
        protected ECSEntity BindEntity;
        private GXGameObject m_GXGO;
        private UniTaskCompletionSource m_UniTaskCompletionSource;
        public GXGameObject GXGO => m_GXGO;
        
        public bool LoadingOver { get; private set; }

        public virtual void Link(ECSEntity ecsEntity)
        {
            BindEntity = ecsEntity;
        }

        protected async UniTaskVoid Load(string path)
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            m_GXGO = new GXGameObject();
            m_UniTaskCompletionSource = new UniTaskCompletionSource();
            LoadingOver = false;
            bool success = await m_GXGO.BindFromAssetAsync(GameObjectPool.Instance, path);
            if (!success)
            {
                m_UniTaskCompletionSource?.TrySetCanceled();
                return;
            }

            LoadingOver = true;
            m_UniTaskCompletionSource?.TrySetResult();
            m_UniTaskCompletionSource = null;
        }
        
        public virtual void Update(float elapseSeconds, float realElapseSeconds)
        {
            
        }

        public virtual void Dispose()
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            m_UniTaskCompletionSource = null;

            m_GXGO.Unbind(GameObjectPool.Instance);
            m_GXGO = null;
            BindEntity = null;
            LoadingOver = false;
        }
        

        public async UniTask WaitLoadOver()
        {
            await m_UniTaskCompletionSource.Task;
        }
        
        public virtual void LocalPosition(LocalPos localPos)
        {
            m_GXGO.localPosition = localPos.Pos;
        }

        public virtual void LocalRotate(LocalRotate localRotate)
        {
            m_GXGO.localRotation = localRotate.Rotate;
        }
        
        public virtual void WolrdPosition(WorldPos worldPos)
        {
            m_GXGO.position = worldPos.Pos;
        }

        public virtual void WorldRotate(WorldRotate worldRotate)
        {
            m_GXGO.rotation = worldRotate.Rotate;
        }

        public virtual void LocalScale(LocalScale localScale)
        {
            m_GXGO.scale = localScale.Scale;
        }
    }
}