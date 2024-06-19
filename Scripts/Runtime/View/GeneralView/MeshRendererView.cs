using Cysharp.Threading.Tasks;
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
            WaitLoadOver().Forget();
        }

        public void Clear()
        {
            m_BindEntity = null;
        }

        private async UniTaskVoid WaitLoadOver()
        {
            await m_GameObjectView.WaitLoadOver();
            SetColor(m_BindEntity.GetMeshRendererColor());
        }

        public void SetColor(MeshRendererColor param)
        {
            if (!m_GameObjectView.LoadingOver) return;
            if (m_MeshRenderer == null)
                m_MeshRenderer = m_GameObjectView.GxGameObjectBase.gameObject.GetComponent<MeshRenderer>();
            if (m_MeshRenderer == null) return;
            var useShareMaterial = m_BindEntity.GetUseShareMaterial();
            if (useShareMaterial != null)
                m_MeshRenderer.sharedMaterial.color = param.Color;
            else
            {
                m_MeshRenderer.material.color = param.Color;
            }
        }
    }
}