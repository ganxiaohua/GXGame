﻿using GameFrame;

namespace GXGame
{
    public class Go3DView : GameObjectView,IMeshRendererColor
    {
        private MeshRendererView m_MeshRendererView;

        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load(ecsEntity.GetAssetPath().path).Forget();
            m_MeshRendererView = ReferencePool.Acquire<MeshRendererView>();
            m_MeshRendererView.Init(BindEntity,this);
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