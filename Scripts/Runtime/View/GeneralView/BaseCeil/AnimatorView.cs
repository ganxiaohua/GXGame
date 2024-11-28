using Cysharp.Threading.Tasks;
using GameFrame;
using UnityEngine;

namespace GXGame
{
    public class AnimatorView : BaseView
    {
        private Animator m_Animator;

        public override void Init(ECSEntity ecsEntity, GameObjectView gameObjectView)
        {
            base.Init(ecsEntity, gameObjectView);
        }

        protected override async UniTask WaitLoadOver()
        {
            await base.WaitLoadOver();
            m_Animator = GameObjectView.GXGO.gameObject.GetComponent<Animator>();
            m_Animator.enabled = true;
        }


        public override void Dispose()
        {
            m_Animator.enabled = false;
            m_Animator = null;
            base.Dispose();
        }

        public void Play(string animationName)
        {
            if (m_Animator == null)
                return;
            m_Animator.Play(animationName);
        }

        public void SetBool(string name, bool b)
        {
            if (m_Animator == null)
                return;
            m_Animator.SetBool(name, b);
        }

        public void SetInteger(string name, int b)
        {
            if (m_Animator == null)
                return;
            m_Animator.SetInteger(name, b);
        }
    }
}