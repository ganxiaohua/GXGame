using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 这是一个早期方案，新方案是直接继承GameObjectProxy
    /// 删除 IWolrdPosition, IWorldRotate, ILocalScale, ILocalPosition, ILocalRotate
    /// 这样更好。因为绝大多数插件需要一个GameObject
    /// </summary>
    public abstract class GameObjectView : IEceView, IWolrdPosition, IWorldRotate, ILocalScale, ILocalPosition, ILocalRotate
    {
        protected EffEntity BindEntity;
        private GameObjectProxy mGxgo;
        private UniTaskCompletionSource m_UniTaskCompletionSource;
        public GameObjectProxy GXGO => mGxgo;

        public bool LoadingOver { get; private set; }

        public virtual void Link(EffEntity effEntity)
        {
            BindEntity = effEntity;
        }

        protected async UniTaskVoid Load(string path)
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            mGxgo = GameObjectProxyPool.Instance.Spawn();
            m_UniTaskCompletionSource = new UniTaskCompletionSource();
            LoadingOver = false;
            bool success = await mGxgo.BindFromAssetAsync(path, Main.ViewLayer);
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
            GameObjectProxyPool.Instance.UnSpawn(mGxgo);
            BindEntity = null;
            LoadingOver = false;
        }

        public async UniTask WaitLoadOver()
        {
            await m_UniTaskCompletionSource.Task;
        }

        public virtual void LocalPosition(LocalPos localPos)
        {
            mGxgo.LocalPosition = localPos.Value;
        }

        public virtual void LocalRotate(LocalRotate localRotate)
        {
            mGxgo.LocalRotation = localRotate.Value;
        }

        public virtual void WolrdPosition(WorldPos worldPos)
        {
            mGxgo.Position = worldPos.Value;
        }

        public virtual void WorldRotate(WorldRotate worldRotate)
        {
            mGxgo.Rotation = worldRotate.Value;
        }

        public virtual void LocalScale(LocalScale localScale)
        {
            mGxgo.scale = localScale.Value;
        }
    }
}