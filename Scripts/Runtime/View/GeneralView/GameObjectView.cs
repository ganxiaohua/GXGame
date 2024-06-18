using Common.Runtime;
using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public class GameObjectView : IEceView, IWolrdPosition, IWorldRotate, ILocalScale,ILocalPosition,ILocalRotate
    {
        protected ECSEntity BindEntity;
        private GXGameObject mGxGameObjectBase;
        private UniTaskCompletionSource m_UniTaskCompletionSource;
        public GXGameObject GxGameObjectBase => mGxGameObjectBase;
        
        public bool LoadingOver { get; private set; }

        public virtual void Link(ECSEntity ecsEntity)
        {
            BindEntity = ecsEntity;
        }

        protected async UniTaskVoid Load(string path)
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            mGxGameObjectBase = new GXGameObject();
            m_UniTaskCompletionSource = new UniTaskCompletionSource();
            LoadingOver = false;
            bool success = await mGxGameObjectBase.BindFromAssetAsync(GameObjectPool.Instance, path);
            if (!success)
            {
                m_UniTaskCompletionSource?.TrySetCanceled();
                return;
            }

            LoadingOver = true;
            m_UniTaskCompletionSource?.TrySetResult();
            m_UniTaskCompletionSource = null;
        }

        public virtual void Clear()
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            m_UniTaskCompletionSource = null;

            mGxGameObjectBase.Unbind(GameObjectPool.Instance);
            mGxGameObjectBase = null;
            BindEntity = null;
            LoadingOver = false;
        }

        public async UniTask WaitLoadOver()
        {
            await m_UniTaskCompletionSource.Task;
        }
        
        public void LocalPosition(LocalPos localPos)
        {
            mGxGameObjectBase.localPosition = localPos.Pos;
        }

        public void LocalRotate(LocalRotate localRotate)
        {
            mGxGameObjectBase.localRotation = localRotate.Rotate;
        }
        
        public void WolrdPosition(WorldPos worldPos)
        {
            mGxGameObjectBase.position = worldPos.Pos;
        }

        public void WorldRotate(WorldRotate worldRotate)
        {
            mGxGameObjectBase.rotation = worldRotate.Rotate;
        }

        public void LocalScale(LocalScale localScale)
        {
            mGxGameObjectBase.scale = localScale.Scale;
        }
    }
}