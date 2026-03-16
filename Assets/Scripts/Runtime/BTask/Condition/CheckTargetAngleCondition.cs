using System;
using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion;
using ParadoxNotion.Design;
using UnityEngine;

namespace GamePlay.Runtime
{
    [Category("AI")]
    public class CheckTargetAngleCondition : ConditionTask
    {
        private EffEntity owner;

        [BlackboardOnly]
        public BBParameter<EffEntity> Tagert;

        public CompareMethod CheckType = CompareMethod.GreaterThan;
        public float LimitedAngle;

        protected override string OnInit()
        {
            owner = blackboard.propertiesBindTarget.GetComponent<ViewEffBindEnitiy>().Entity;
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
            var ownerView = owner.GetView().GetData();
            var targetView = Tagert.value.GetView().GetData();
            var ownerPos = ownerView.Position;
            var targetPos = targetView.Position;
            var forward = ownerView.transform.forward;
            ownerPos.y = 0;
            targetPos.y = 0;
            forward.y = 0;
            var angle = Vector3.Angle(targetPos - ownerPos, forward);
            return OperationTools.Compare(angle, LimitedAngle, CheckType, 0.01f);
        }
    }
}