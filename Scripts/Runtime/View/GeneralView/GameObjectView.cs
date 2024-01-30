using Common.Runtime;
using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;
using UnityEngine.UIElements;

namespace GXGame
{
    public class GameObjectView : IEceView
    {
        private ECSEntity m_BindEntity;
        private GameObject3D m_GameObjectBase;
        private UniTaskCompletionSource UniTaskCompletionSource;
        public GameObject3D GameObjectBase => m_GameObjectBase;
        private EntityComponentNumericalChange<WorldPos> m_PosDelegate;
        private EntityComponentNumericalChange<WorldRotate> m_RotDelegate;
        private EntityComponentNumericalChange<LocalScale> m_LocalScale;

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
            WolrdPosition(m_BindEntity.GetWorldPos(), m_BindEntity);
            WorldRotate(m_BindEntity.GetWorldRotate(), m_BindEntity);
            LocalScale(m_BindEntity.GetLocalScale(), m_BindEntity);
            m_PosDelegate = WolrdPosition;
            m_RotDelegate = WorldRotate;
            m_LocalScale = LocalScale;
            ViewBindEventClass.WorldPosEntityComponentNumericalChange += m_PosDelegate;
            ViewBindEventClass.WorldRotateEntityComponentNumericalChange += m_RotDelegate;
            ViewBindEventClass.LocalScaleEntityComponentNumericalChange += m_LocalScale;
        }

        public virtual void Clear()
        {
            if (UniTaskCompletionSource != null)
            {
                UniTaskCompletionSource.TrySetCanceled();
            }

            m_GameObjectBase.Unbind(GameObjectPool.Instance);
            ViewBindEventClass.WorldPosEntityComponentNumericalChange -= m_PosDelegate;
            ViewBindEventClass.WorldRotateEntityComponentNumericalChange -= m_RotDelegate;
            ViewBindEventClass.LocalScaleEntityComponentNumericalChange -= m_LocalScale;
            m_GameObjectBase = null;
            m_PosDelegate = null;
            m_RotDelegate = null;
            m_LocalScale = null;
            m_BindEntity = null;
        }

        public async UniTask WaitLoadOver()
        {
            await UniTaskCompletionSource.Task;
            UniTaskCompletionSource = null;
        }

        private void WolrdPosition(WorldPos worldPos, ECSEntity ecsEntity)
        {
            if (worldPos == null || m_BindEntity.ID != ecsEntity.ID)
                return;
            m_GameObjectBase.transform.position = worldPos.Pos;
        }


        private void WorldRotate(WorldRotate worldRotate, ECSEntity ecsEntity)
        {
            if (worldRotate == null || m_BindEntity.ID != ecsEntity.ID)
                return;
            m_GameObjectBase.transform.rotation = worldRotate.Rotate;
        }

        private void LocalScale(LocalScale localScale, ECSEntity ecsEntity)
        {
            if (localScale == null || m_BindEntity.ID != ecsEntity.ID)
                return;
            m_GameObjectBase.transform.localScale = localScale.Scale;
        }
    }
}