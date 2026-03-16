using System;
using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;

namespace GamePlay.Runtime
{
    [Category("AI")]
    public class CheckTargetCondition : ConditionTask
    {
        public Type ComponentType;

        [BlackboardOnly]
        EffEntity Owner;

        [BlackboardOnly]
        public BBParameter<EffEntity> Tagert;

        private Group searchGroup;
        private float sqlTrackDistance;

        protected override string OnInit()
        {
            var x = blackboard.variables;
            Owner = blackboard.propertiesBindTarget.GetComponent<ViewEffBindEnitiy>().Entity;
            var indexOf = ComponentsID2Type.ComponentsTypes.IndexOf(ComponentType);
            Matcher matcher = Matcher.SetAll(indexOf);
            searchGroup = Owner.world.GetGroup(matcher);
            var monster = Owner.GetUnit().BehaviorTable_Ref.BattleBehavior_Ref;
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
            var ownerViewData = Owner.GetView().GetData();
            foreach (var eff in searchGroup)
            {
                var pos = eff.GetView().GetData().Position;
                var owerPos = ownerViewData.Position;
                float sqrdistance = (pos - owerPos).sqrMagnitude;
                if (OperationTools.Compare(sqrdistance, sqlTrackDistance, CompareMethod.LessThan, 0.01f))
                {
                    Tagert.value = eff;
                }
                else
                {
                    Tagert.value = null;
                }
            }

            return true;
        }
    }
}