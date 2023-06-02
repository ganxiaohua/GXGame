using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;
using UnityEngine.UIElements;

namespace GXGame
{
    public class GameObjectView : IEceView
    {
        private ECSEntity m_BindEntity;
        private GameObjectObjectBase m_GameObjectBase;
        public GameObjectObjectBase GameObjectBase => m_GameObjectBase;
        private EntityComponentNumericalChange<WorldPos> m_PosDelegate;
        private EntityComponentNumericalChange<WorldRotate> m_RotDelegate;
        private EntityComponentNumericalChange<LocalScale> m_LocalScale;
        private EntityComponentNumericalChange<WorldOffsetPos> m_WorldOffsetPos;

        public void Link(ECSEntity ecsEntity)
        {
            m_BindEntity = ecsEntity;
            Init(m_BindEntity.GetAssetPath().Path);
        }

        public void Init(string path)
        {
            m_GameObjectBase = ObjectPoolFactory.GetObject(path);
            WolrdPosition(m_BindEntity.GetWorldPos(), m_BindEntity);
            WorldRotate(m_BindEntity.GetWorldRotate(), m_BindEntity);
            m_PosDelegate = WolrdPosition;
            m_RotDelegate = WorldRotate;
            m_LocalScale = LocalScale;
            m_WorldOffsetPos = WorldOffsetPos;
            ViewBindEventClass.WorldPosEntityComponentNumericalChange -= m_PosDelegate;
            ViewBindEventClass.WorldPosEntityComponentNumericalChange += m_PosDelegate;
            ViewBindEventClass.WorldRotateEntityComponentNumericalChange -= m_RotDelegate;
            ViewBindEventClass.WorldRotateEntityComponentNumericalChange += m_RotDelegate;
            ViewBindEventClass.LocalScaleEntityComponentNumericalChange -= m_LocalScale;
            ViewBindEventClass.LocalScaleEntityComponentNumericalChange += m_LocalScale;
            ViewBindEventClass.WorldOffsetPosEntityComponentNumericalChange -= m_WorldOffsetPos;
            ViewBindEventClass.WorldOffsetPosEntityComponentNumericalChange += m_WorldOffsetPos;
        }

        public void Clear()
        {
            ObjectPoolFactory.RecycleObject(m_GameObjectBase);
            ViewBindEventClass.WorldPosEntityComponentNumericalChange -= m_PosDelegate;
            ViewBindEventClass.WorldRotateEntityComponentNumericalChange -= m_RotDelegate;
            ViewBindEventClass.LocalScaleEntityComponentNumericalChange -= m_LocalScale;
            ViewBindEventClass.WorldOffsetPosEntityComponentNumericalChange -= m_WorldOffsetPos;
            m_PosDelegate = null;
            m_RotDelegate = null;
            m_LocalScale = null;
            m_BindEntity = null;
        }

        public async UniTask WaitLoadOver()
        {
            await m_GameObjectBase.LoadOver();
        }

        private void WolrdPosition(WorldPos worldPos, ECSEntity ecsEntity)
        {
            if (m_BindEntity.ID != ecsEntity.ID)
                return;
            m_GameObjectBase.WorldPos = worldPos.Pos;
        }
        
        public void WorldOffsetPos(WorldOffsetPos worldOffsetPos, ECSEntity ecsEntity)
        {
            if (m_BindEntity.ID != ecsEntity.ID)
                return;
            m_GameObjectBase.WorldPos += worldOffsetPos.OffsetPos;
        }

        private void WorldRotate(WorldRotate worldRotate, ECSEntity ecsEntity)
        {
            if (m_BindEntity.ID != ecsEntity.ID)
                return;
            m_GameObjectBase.WorldRot = Quaternion.Euler(worldRotate.Rotate);
        }

        private void LocalScale(LocalScale localScale, ECSEntity ecsEntity)
        {
            if (m_BindEntity.ID != ecsEntity.ID)
                return;
            m_GameObjectBase.LocalScale = localScale.Scale;
        }
    }
}