using System;
using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GamePlay.Runtime
{
    [Category("AI")]
    public class MoveTargetTask : ActionTask
    {
        private EffEntity owner;

        [BlackboardOnly]
        public BBParameter<EffEntity> Tagert;

        protected override string OnInit()
        {
            owner = blackboard.propertiesBindTarget.GetComponent<ViewEffBindEnitiy>().Entity;
            return null;
        }

        protected override void OnExecute()
        {
            var ownerPos = owner.GetView().GetData().Position;
            var dir = Tagert.value.GetView().GetData().Position - ownerPos;
            dir.y = 0;
            owner.SetMoveDirectionComp(dir.normalized);
            EndAction(true);
        }
    }
}