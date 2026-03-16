using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GamePlay.Runtime
{
    [Category("AI")]
    public class CheckTargetDistanceCondition : ConditionTask
    {
        private EffEntity Owner;

        [BlackboardOnly]
        public BBParameter<EffEntity> Tagert;

        public CompareMethod CheckType = CompareMethod.GreaterThan;
        public BehaviorDistanceType DistanceType;
        // private MonsterItem monster;

        private float sqlAtkDistance;
        private float sqlTrackDistance;

        protected override string OnInit()
        {
            Owner = blackboard.propertiesBindTarget.GetComponent<ViewEffBindEnitiy>().Entity;
            var monster = Owner.GetUnit().BehaviorTable_Ref.BattleBehavior_Ref;
            sqlAtkDistance = monster.AtkDistance * monster.AtkDistance;
            sqlTrackDistance = monster.TrackDistance * monster.TrackDistance;
            return null;
        }

        protected override void OnEnable()
        {
        }


        protected override void OnDisable()
        {
        }

        protected override bool OnCheck()
        {
            if (Tagert == null || Tagert.value == null || Tagert.value.HasComponent<DieComp>())
                return false;
            var ownerPos = Owner.GetView().GetData().Position;
            var dir = Tagert.value.GetView().GetData().Position - ownerPos;
            float sqrdistance = dir.sqrMagnitude;
            if (DistanceType == BehaviorDistanceType.Track)
            {
                return OperationTools.Compare(sqrdistance, sqlTrackDistance, CheckType, 0.01f);
            }
            else if (DistanceType == BehaviorDistanceType.Atk)
            {
                return OperationTools.Compare(sqrdistance, sqlAtkDistance, CheckType, 0.01f);
            }

            return false;
        }
    }
}