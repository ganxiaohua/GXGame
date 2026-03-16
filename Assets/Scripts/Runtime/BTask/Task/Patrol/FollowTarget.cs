using System.Collections.Generic;
using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace GamePlay.Runtime
{
    [Category("AI")]
    public class FollowTarget : ActionTask
    {
        private EffEntity owner;

        protected override string OnInit()
        {
            owner = blackboard.propertiesBindTarget.GetComponent<ViewEffBindEnitiy>().Entity;
            return null;
        }

        protected override void OnExecute()
        {
            EndAction();
        }

        private void OnPath(Path path)
        {
        }
    }
}