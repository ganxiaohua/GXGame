using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 需要依附GameObjectView,无法单独存在
    /// </summary>
    public class MeshRendererView : BaseView
    {
        private MeshRenderer m_MeshRenderer;

        public override void Init(ECSEntity ecsEntity, GameObjectView gameObjectView)
        {
            base.Init(ecsEntity, gameObjectView);
        }

        public override void Clear()
        {
            base.Clear();
            m_MeshRenderer = null;
        }

        protected override async UniTask WaitLoadOver()
        {
            await base.WaitLoadOver();
            SetColor(BindEntity.GetMeshRendererColor());
        }

        public void SetColor(MeshRendererColor param)
        {
            if (!GameObjectView.LoadingOver) return;
            if (m_MeshRenderer == null)
                m_MeshRenderer = GameObjectView.GXGO.gameObject.GetComponent<MeshRenderer>();
            if (m_MeshRenderer == null) return;
            var useShareMaterial = BindEntity.GetUseShareMaterial();
            if (useShareMaterial != null)
                m_MeshRenderer.sharedMaterial.color = param.Color;
            else
            {
                m_MeshRenderer.material.color = param.Color;
            }
        }
    }
}