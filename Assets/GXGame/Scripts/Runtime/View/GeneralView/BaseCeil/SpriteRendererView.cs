using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GXGame
{
    public class SpriteRendererView : BaseView
    {
        private SpriteRenderer m_SpriteRenderer;

        public override void Init(EffEntity effEntity, GameObjectView gameObjectView)
        {
            base.Init(effEntity, gameObjectView);
        }

        protected override async UniTask WaitLoadOver()
        {
            await base.WaitLoadOver();
            m_SpriteRenderer = GameObjectView.GXGO.gameObject.GetComponentInChildren<SpriteRenderer>();
        }


        public override void Dispose()
        {
            base.Dispose();
            m_SpriteRenderer = null;
        }

        public void SetSprite(Sprite sprite)
        {
            if (m_SpriteRenderer == null)
                return;
            m_SpriteRenderer.sprite = sprite;
        }
    }
}