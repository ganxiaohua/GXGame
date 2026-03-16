using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class ManView : LogicView
    {
        private Animator animator;
        private int curAnimatorId;
        public GameObjectProxy ViewPoint { get; private set; }


        public override void Initialize(object initData)
        {
            base.Initialize(initData);
            ViewPoint = ReferencePool.Acquire<GameObjectProxy>();
            ViewPoint.Initialize();
            ViewPoint.transform.SetParent(transform);
            ViewPoint.Position = transform.position;
        }

        protected override async UniTask LoadLogic()
        {
            await base.LoadLogic();
            ViewPoint.LocalPosition = logicData.Center;
        }

        protected override void OnAfterBind(GameObject go)
        {
            base.OnAfterBind(go);
            animator = go.gameObject.GetComponentInChildren<Animator>();
        }


        public override void TickActive(float elapseSeconds, float realElapseSeconds)
        {
            base.TickActive(elapseSeconds, realElapseSeconds);
            if (animator == null)
                return;
            var data = ForceAimator();
            if (!data.isPlay)
                data = DieAimator();
            if (!data.isPlay)
                data = DamageAimator();
            if (!data.isPlay)
                data = OperatedAimator();
            if (!data.isPlay)
                data = RunAimator();
            if (data.isPlay && curAnimatorId != data.curTag)
            {
                curAnimatorId = data.curTag;
                animator.CrossFadeInFixedTime(curAnimatorId, 0.15f);
            }
        }

        /// <summary>
        /// 强制动画
        /// </summary>
        /// <returns></returns>
        private (int curTag, bool isPlay) ForceAimator()
        {
            int curTag = curAnimatorId;
            bool isPlay = false;
            if (BindEntity.HasComponent<ForceAnimationComp>())
            {
                var comp = BindEntity.GetForceAnimationComp();
                isPlay = true;
                curTag = AnimatorName.AnimaHashIndex[comp.Value];
            }

            return (curTag, isPlay);
        }

        private (int curTag, bool isPlay) DieAimator()
        {
            int curTag = curAnimatorId;
            bool isPlay = false;
            if (BindEntity.HasComponent<DieComp>())
            {
                isPlay = true;
                curTag = AnimatorName.Die;
            }

            return (curTag, isPlay);
        }

        private (int curTag, bool isPlay) OperatedAimator()
        {
            int curTag = curAnimatorId;
            bool isPlay = false;
            if (OnGround())
            {
                if (BindEntity.HasComponent<OperatedCountdownComp>())
                {
                    var beingOperated = BindEntity.GetOperatedCountdownComp();
                    var curAnimationItem = Tables.Instance.AnimationTable.GetOrDefault(beingOperated.Value);
                    curTag = AnimatorName.AnimaHashIndex[curAnimationItem.AnimationHashIndex];
                    animator.speed = 1;
                    isPlay = true;
                }
            }
            else if (IsFly())
            {
                curTag = AnimatorName.Fly_1;
                animator.speed = 1;
                isPlay = true;
            }

            return (curTag, isPlay);
        }

        private (int curTag, bool isPlay) DamageAimator()
        {
            int curTag = curAnimatorId;
            bool isPlay = false;
            if (BindEntity.HasComponent<BeAttackBuffComp>())
            {
                isPlay = true;
                curTag = AnimatorName.Damage;
            }

            return (curTag, isPlay);
        }

        private (int curTag, bool isPlay) RunAimator()
        {
            var groundCollision = BindEntity.GetGroundCollisionComp().Value;
            int curTag = curAnimatorId;
            //如果操作倒计时存在,则优先级较低的移动指令跳过
            if (BindEntity.HasComponent<OperatedCountdownComp>())
                return (curTag, false);
            if (!groundCollision.OnGround)
            {
                curTag = AnimatorName.Jump_Air;
            }
            else
            {
                var dir = BindEntity.GetMoveDirectionComp().Value;
                var turnDir = BindEntity.GetTurnDirectionComp().Value;
                animator.speed = BindEntity.GetRunSpeedUpComp().Value;
                var speed = BindEntity.GetMoveSpeedComp().Value;
                curTag = (dir == Vector3.zero && turnDir == Vector3.zero ? AnimatorName.Idle :
                        speed > ConstData.RunAnimationPoint ? AnimatorName.Run : AnimatorName.Walk);
            }

            return (curTag, true);
        }

        private bool OnGround()
        {
            var groundCollision = BindEntity.GetGroundCollisionComp();
            return groundCollision.Value.OnGround;
        }

        private bool IsFly()
        {
            return BindEntity.HasComponent<FlyComp>();
        }

        public override void Dispose()
        {
            if (ViewPoint != null)
                ReferencePool.Release(ViewPoint);
            ViewPoint = null;
            base.Dispose();
        }
    }
}