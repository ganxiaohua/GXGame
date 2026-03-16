using System;
using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GamePlay.Runtime
{
    [Category("AI")]
    public class StopMoveTask : ActionTask
    {
        private EffEntity owner;

        protected override string OnInit()
        {
            owner = blackboard.propertiesBindTarget.GetComponent<ViewEffBindEnitiy>().Entity;
            return null;
        }

        protected override void OnExecute()
        {
            owner.SetMoveDirectionComp(Vector3.zero);
            EndAction(true);
        }
    }
}