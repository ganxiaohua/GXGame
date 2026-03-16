using Cysharp.Threading.Tasks;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class MonsterView : LogicView
    {
        private Animator animator;
        private int curAnimatorId;


        public override void Initialize(object initData)
        {
            base.Initialize(initData);
        }

        protected override async UniTask LoadLogic()
        {
            await base.LoadLogic();
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

        private (int curTag, bool isPlay) RunAimator()
        {
            var dir = BindEntity.GetMoveDirectionComp().Value;
            var turnDir = BindEntity.GetTurnDirectionComp().Value;
            int curTag = curAnimatorId;
            animator.speed = BindEntity.GetRunSpeedUpComp().Value;
            curTag = (dir == Vector3.zero && turnDir == Vector3.zero ? AnimatorName.Idle : AnimatorName.Run);
            return (curTag, true);
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
            if (BindEntity.HasComponent<OperatedCountdownComp>())
            {
                var beingOperated = BindEntity.GetOperatedCountdownComp();
                var curAnimationItem = Tables.Instance.AnimationTable.GetOrDefault(beingOperated.Value);
                curTag = AnimatorName.AnimaHashIndex[curAnimationItem.AnimationHashIndex];
                animator.speed = 1;
                isPlay = true;
            }

            return (curTag, isPlay);
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}