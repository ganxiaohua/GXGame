using GameFrame;
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
            ViewBindEventClass.MeshRendererColorEntityComponentNumericalChange += SetColor;
            WaitLoadOver();
        }

        public void Clear()
        {
            ViewBindEventClass.MeshRendererColorEntityComponentNumericalChange -= SetColor;
            m_BindEntity = null;
        }

        private async void WaitLoadOver()
        {
            await m_GameObjectView.WaitLoadOver();
            SetColor(m_BindEntity.GetMeshRendererColor(), m_BindEntity);
        }

        private void SetColor(GXGame.MeshRendererColor param, ECSEntity ecsEntity)
        {
            if (ecsEntity == null || ecsEntity.ID != m_BindEntity.ID)
                return;
            if (m_MeshRenderer == null)
                m_MeshRenderer = m_GameObjectView.GameObjectBase.gameObject.GetComponent<MeshRenderer>();
            if (m_MeshRenderer != null)
            {
                m_MeshRenderer.material.color = param.Color;
            }
        }
    }
}