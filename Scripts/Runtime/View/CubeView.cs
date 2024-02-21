using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class CubeView : GameObjectView,IMeshRendererColor
    {
        private ECSEntity m_BindEntity;
        private MeshRendererView m_MeshRendererView;

        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            m_BindEntity = ecsEntity;
            m_MeshRendererView = ReferencePool.Acquire<MeshRendererView>();
            m_MeshRendererView.Init(m_BindEntity,this);
        }
        

        public override void Clear()
        {
            base.Clear();
            ReferencePool.Release(m_MeshRendererView);
        }

        public void MeshRendererColor(MeshRendererColor meshRendererColor)
        {
            m_MeshRendererView.SetColor(meshRendererColor);
        }
    }
}