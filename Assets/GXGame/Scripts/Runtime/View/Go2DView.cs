using GameFrame.Runtime;
using UnityEngine;


namespace GXGame
{
    public class Go2DView : GameObjectView, IFaceDirection, IAtkComp, IAtkOverComp
    {
        private AnimatorView animator;
        private SpriteRendererView spriterenderer;
        private Vector2 curdir = new Vector2(0, -1);

        public override void Link(EffEntity effEntity)
        {
            base.Link(effEntity);
            Load(effEntity.GetAssetPath().Value).Forget();
            spriterenderer = ReferencePool.Acquire<SpriteRendererView>();
            spriterenderer.Init(effEntity, this);
            animator = ReferencePool.Acquire<AnimatorView>();
            animator.Init(effEntity, this);
        }

        public override void Dispose()
        {
            ReferencePool.Release(spriterenderer);
            ReferencePool.Release(animator);
            animator = null;
            spriterenderer = null;
            base.Dispose();
        }

        public void FaceDirection(FaceDirection faceDirection)
        {
            if (animator == null)
                return;
            var dir = faceDirection.Value;
            var scale = BindEntity.GetLocalScale().Value;
            if (dir != Vector3.zero)
            {
                curdir = dir;
                animator.SetBool("Stop", false);
                animator.SetInteger("State", 1);
                GXGO.scale = dir.x switch
                {
                        > 0 => new Vector3(scale.x, scale.y, scale.z),
                        < 0 => new Vector3(-scale.x, scale.y, scale.z),
                        _ => GXGO.scale
                };
                if (dir.y < 0)
                    animator.SetInteger("Direction", 1);
                else if (dir.y > 0)
                    animator.SetInteger("Direction", 3);

                if (dir.x > 0 || dir.x < 0)
                {
                    animator.SetInteger("Direction", 2);
                }
            }
            else
            {
                animator.SetBool("Stop", true);
            }
        }

        public void AtkComp(AtkStartComp atkStartComp)
        {
            animator.SetBool("Stop", true);
            if (curdir.y < 0)
            {
                animator.SetInteger("State", 2);
            }
            else if (curdir.y > 0)
            {
                animator.SetInteger("State", 4);
            }
            else if (curdir.x != 0)
            {
                animator.SetInteger("State", 3);
            }
        }

        public void AtkOverComp(AtkOverComp atkComp)
        {
            animator.SetBool("Stop", true);
            animator.SetInteger("State", 1);
        }
    }
}