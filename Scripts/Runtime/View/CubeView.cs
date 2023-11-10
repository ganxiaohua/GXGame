using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class CubeView : IEceView
    {
        private ECSEntity m_BindEntity;
        private GameObjectView m_GameObjectView;
        private MeshRendererView m_MeshRendererView;

        public void Link(ECSEntity ecsEntity)
        {
            m_BindEntity = ecsEntity;
            m_GameObjectView = ReferencePool.Acquire<GameObjectView>();
            m_GameObjectView.Link(m_BindEntity);
            m_MeshRendererView = ReferencePool.Acquire<MeshRendererView>();
            m_MeshRendererView.Init(m_BindEntity,m_GameObjectView);
        }

        public void Clear()
        {
        }
        
    }
}