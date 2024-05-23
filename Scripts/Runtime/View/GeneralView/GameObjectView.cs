using Common.Runtime;
using Cysharp.Threading.Tasks;
using GameFrame;

namespace GXGame
{
    public class GameObjectView : IEceView, IWolrdPosition, IWorldRotate, ILocalScale
    {
        private ECSEntity m_BindEntity;
        private GameObject3D m_GameObjectBase;
        private UniTaskCompletionSource m_UniTaskCompletionSource;
        public GameObject3D GameObjectBase => m_GameObjectBase;
        
        public bool LoadingOver { get; private set; }

        public virtual void Link(ECSEntity ecsEntity)
        {
            m_BindEntity = ecsEntity;
            Load(m_BindEntity.GetAssetPath().Path).Forget();
        }

        private async UniTaskVoid Load(string path)
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            m_GameObjectBase = new GameObject3D();
            m_UniTaskCompletionSource = new UniTaskCompletionSource();
            LoadingOver = false;
            bool success = await m_GameObjectBase.BindFromAssetAsync(GameObjectPool.Instance, path);
            if (!success)
            {
                m_UniTaskCompletionSource?.TrySetCanceled();
                return;
            }

            LoadingOver = true;
            m_UniTaskCompletionSource.TrySetResult();
            m_UniTaskCompletionSource = null;
            WolrdPosition(m_BindEntity.GetWorldPos());
            WorldRotate(m_BindEntity.GetWorldRotate());
            LocalScale(m_BindEntity.GetLocalScale());
        }

        public virtual void Clear()
        {
            m_UniTaskCompletionSource?.TrySetCanceled();
            m_UniTaskCompletionSource = null;

            m_GameObjectBase.Unbind(GameObjectPool.Instance);
            m_GameObjectBase = null;
            m_BindEntity = null;
            LoadingOver = false;
        }

        public async UniTask WaitLoadOver()
        {
            await m_UniTaskCompletionSource.Task;
        }


        public void WolrdPosition(WorldPos worldPos)
        {
            if (!LoadingOver) return;
            m_GameObjectBase.transform.position = worldPos.Pos;
        }

        public void WorldRotate(WorldRotate worldRotate)
        {
            if (!LoadingOver) return;
            m_GameObjectBase.transform.rotation = worldRotate.Rotate;
        }

        public void LocalScale(LocalScale localScale)
        {
            if (!LoadingOver) return;
            m_GameObjectBase.transform.localScale = localScale.Scale;
        }
    }
}