using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GXGame
{
    [Category("怪物AI")]
    [Description("判断和目标的距离")]
    public class CheckDistance : ConditionTask
    {
        private ECSEntity owner;
        private World world;

        public float maxDistance;

        //Use for initialization. This is called only once in the lifetime of the task.
        //Return null if init was successfull. Return an error string otherwise
        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            return null;
        }

        //Called whenever the condition gets enabled.
        protected override void OnEnable()
        {
        }

        //Called whenever the condition gets disabled.
        protected override void OnDisable()
        {
        }

        //Called once per frame while the condition is active.
        //Return whether the condition is success or failure.
        protected override bool OnCheck()
        {
            var targetPos = Vector3.zero;
            if (owner.HasComponent(Components.TargetPos))
            {
                targetPos = owner.GetTargetPos().Value;
            }
            var curPos = owner.GetWorldPos().Value;
            return Vector3.Distance(curPos, targetPos) <= maxDistance;
        }
    }
}