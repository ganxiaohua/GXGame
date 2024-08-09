using GameFrame;
using UnityEngine;


namespace GXGame
{
    public class Go2DView : GameObjectView
    {
        private AnimatorView animator;
        private SpriteRendererView spriterenderer;
        public override void Link(ECSEntity ecsEntity)
        {
            base.Link(ecsEntity);
            Load(ecsEntity.GetAssetPath().path).Forget();
            spriterenderer = ReferencePool.Acquire<SpriteRendererView>();
            spriterenderer.Init(ecsEntity,this);
            animator = ReferencePool.Acquire<AnimatorView>();
            animator.Init(ecsEntity,this);
        }
        
        
        public override void Update(float elapseSeconds, float realElapseSeconds)
        {
            base.Update(elapseSeconds,realElapseSeconds);
            if(animator==null)
                return;
            var dir = BindEntity.GetMoveDirection().Dir;
            if (dir.x > 0)
            {
                GXGO.gameObject.transform.localScale = new Vector3(1, 1, 1);
                animator.Play("Walk");
            }
            else if (dir.x < 0)
            {
                GXGO.gameObject.transform.localScale = new Vector3(-1, 1, 1);
                animator.Play("Walk");
            }
            else if (dir.y > 0)
            {
                animator.Play("ForwardWalk");
            }else if (dir.y < 0)
            {
                animator.Play("BelowWalk");
            }

            if (dir != Vector3.zero)
            {
                animator.SetBool("Stop",false);
            }
            else if (dir == Vector3.zero)
            {
                animator.SetBool("Stop",true);
            }
        }
        
        public override void Clear()
        {
            base.Clear();
            ReferencePool.Release(spriterenderer);
            ReferencePool.Release(animator);
        }
    }
}