﻿using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 需要依附GameObjectView,无法单独存在
    /// </summary>
    public class MeshRendererView : IReference
    {
        private ECSEntity m_BindEntity;
        private GameObjectView m_GameObjectView;
        private MeshRenderer m_MeshRenderer;

        public void Init(ECSEntity ecsEntity, GameObjectView gameObjectView)
        {
            m_GameObjectView = gameObjectView;
            m_BindEntity = ecsEntity;
            WaitLoadOver();
        }

        public void Clear()
        {
            m_BindEntity = null;
        }

        private async void WaitLoadOver()
        {
            await m_GameObjectView.WaitLoadOver();
            SetColor();
        }

        private void SetColor()
        {
            if (m_MeshRenderer == null)
                m_MeshRenderer = m_GameObjectView.GameObjectBase.GetComponent<MeshRenderer>();
            if (m_MeshRenderer != null)
            {
                m_MeshRenderer.material.color = m_BindEntity.GetMeshRendererColor().Color;
            }
        }
    }
}