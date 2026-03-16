using System;
using System.Collections.Generic;
using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;

namespace GamePlay.Runtime
{
    [Category("AI")]
    public class PatrolTask : ActionTask
    {
        private EffEntity owner;

        [BlackboardOnly]
        public BBParameter<List<Vector3>> findPath;

        private EffEntityView view;
        private int index;


        protected override string OnInit()
        {
            owner = blackboard.propertiesBindTarget.GetComponent<ViewEffBindEnitiy>().Entity;
            view = owner.GetView().GetData();
            return null;
        }

        protected override void OnExecute()
        {
            if (findPath.value.Count == 0)
            {
                EndAction(true);
                return;
            }

            if (Vector3.SqrMagnitude(findPath.value[index] - view.Position) <= 0.5f)
            {
                index++;
            }

            if (index >= findPath.value.Count)
            {
                EndAction(true);
                return;
            }

            var dir = findPath.value[index] - view.Position;
            owner.SetMoveDirectionComp(dir);
            owner.SetTurnDirectionComp(dir);
            EndAction(true);
        }
    }
}