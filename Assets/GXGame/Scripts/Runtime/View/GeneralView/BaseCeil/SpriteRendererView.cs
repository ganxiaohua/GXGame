using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class SpriteRendererView : BaseView
    {
        private SpriteRenderer m_SpriteRenderer;

        public override void Init(ECSEntity ecsEntity, GameObjectView gameObjectView)
        {
            base.Init(ecsEntity, gameObjectView);
        }

        protected override async UniTask WaitLoadOver()
        {
            await base.WaitLoadOver();
            m_SpriteRenderer = GameObjectView.GXGO.gameObject.GetComponent<SpriteRenderer>();
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