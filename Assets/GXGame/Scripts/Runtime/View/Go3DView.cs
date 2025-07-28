using GameFrame.Runtime;

namespace GXGame
{
    public class Go3DView : GameObjectView, IMeshRendererColor
    {
        private MeshRendererView m_MeshRendererView;

        public override void Link(EffEntity effEntity)
        {
            base.Link(effEntity);
            Load(effEntity.GetAssetPath().Value).Forget();
            m_MeshRendererView = ReferencePool.Acquire<MeshRendererView>();
            m_MeshRendererView.Init(BindEntity, this);
        }

        public override void Dispose()
        {
            base.Dispose();
            ReferencePool.Release(m_MeshRendererView);
        }

        public void MeshRendererColor(MeshRendererColor meshRendererColor)
        {
            m_MeshRendererView.SetColor(meshRendererColor);
        }
    }
}