using Common.Runtime;
using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public abstract class GameObjectView : IEceView, IWolrdPosition, IWorldRotate, ILocalScale, ILocalPosition, ILocalRotate
    {
        protected EffEntity BindEntity;
        private GXGameObject m_GXGO;
        private UniTaskCompletionSource m_UniTaskCompletionSource;
        public GXGameObject GXGO => m_GXGO;

        public bool LoadingOver { get; private set; }

        public virtual void Link(EffEntity effEntity)
        {
            BindEntity = effEntity;
        }

        protected async UniTaskVoid Load(string path)
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            m_GXGO = new GXGameObject();
            m_UniTaskCompletionSource = new UniTaskCompletionSource();
            LoadingOver = false;
            bool success = await m_GXGO.BindFromAssetAsync(path, Main.ViewLayer);
            if (!success)
            {
                m_UniTaskCompletionSource?.TrySetCanceled();
                return;
            }

            LoadingOver = true;
            m_UniTaskCompletionSource?.TrySetResult();
            m_UniTaskCompletionSource = null;
            if (BindEntity.GetLocalPos() != null)
                LocalPosition(BindEntity.GetLocalPos());
            if (BindEntity.GetLocalRotate() != null)
                LocalRotate(BindEntity.GetLocalRotate());
            if (BindEntity.GetLocalScale() != null)
                LocalScale(BindEntity.GetLocalScale());
            if (BindEntity.GetWorldRotate() != null)
                WorldRotate(BindEntity.GetWorldRotate());
            if (BindEntity.GetWorldPos() != null)
                WolrdPosition(BindEntity.GetWorldPos());
        }

        public virtual void OnUpdate(float elapseSeconds, float realElapseSeconds)
        {
        }

        public virtual void Dispose()
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            m_UniTaskCompletionSource = null;

            m_GXGO.Unbind();
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
            m_GXGO.localPosition = localPos.Value;
        }

        public virtual void LocalRotate(LocalRotate localRotate)
        {
            m_GXGO.localRotation = localRotate.Value;
        }

        public virtual void WolrdPosition(WorldPos worldPos)
        {
            m_GXGO.position = worldPos.Value;
        }

        public virtual void WorldRotate(WorldRotate worldRotate)
        {
            m_GXGO.rotation = worldRotate.Value;
        }

        public virtual void LocalScale(LocalScale localScale)
        {
            m_GXGO.scale = localScale.Value;
        }
    }
}