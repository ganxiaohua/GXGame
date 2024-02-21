using Common.Runtime;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;
using UnityEngine.UIElements;

namespace GXGame
{
    public class GameObjectView : IEceView,IWolrdPosition,IWorldRotate,ILocalScale
    {
        private ECSEntity m_BindEntity;
        private GameObject3D m_GameObjectBase;
        private UniTaskCompletionSource UniTaskCompletionSource;
        public GameObject3D GameObjectBase => m_GameObjectBase;

        public virtual void Link(ECSEntity ecsEntity)
        {
            m_BindEntity = ecsEntity;
            Init(m_BindEntity.GetAssetPath().Path).Forget();
        }

        public async UniTaskVoid Init(string path)
        {
            m_GameObjectBase = new GameObject3D();
            UniTaskCompletionSource = new UniTaskCompletionSource();
            bool success = await m_GameObjectBase.BindFromAssetAsync(GameObjectPool.Instance, path);
            if (!success)
            {
                return;
            }
            UniTaskCompletionSource.TrySetResult();
            WolrdPosition(m_BindEntity.GetWorldPos());
            WorldRotate(m_BindEntity.GetWorldRotate());
            LocalScale(m_BindEntity.GetLocalScale());
        }

        public virtual void Clear()
        {
            if (UniTaskCompletionSource != null)
            {
                UniTaskCompletionSource.TrySetCanceled();
            }

            m_GameObjectBase.Unbind(GameObjectPool.Instance);
            m_GameObjectBase = null;
            m_BindEntity = null;
        }

        public async UniTask WaitLoadOver()
        {
            await UniTaskCompletionSource.Task;
            UniTaskCompletionSource = null;
        }
        

        public void WolrdPosition(WorldPos worldPos)
        {
            m_GameObjectBase.transform.position = worldPos.Pos;
        }

        public void WorldRotate(WorldRotate worldRotate)
        {
            m_GameObjectBase.transform.rotation = worldRotate.Rotate;
        }

        public void LocalScale(LocalScale localScale)
        {
            m_GameObjectBase.transform.localScale = localScale.Scale;
        }
    }
}