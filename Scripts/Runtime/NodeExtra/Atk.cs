using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GXGame
{
    [Category("怪物AI")]
    [Description("攻击")]
    public class Atk : ActionTask
    {
        private ECSEntity owner;
        private World world;

        protected override string OnInit()
        {
            owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            return null;
        }

        protected override void OnExecute()
        {
            var skillEntity = world.AddChild<SkillEntity>();
            skillEntity.AddSkillComponent(1);
            skillEntity.AddViewType(typeof(GoBaseView));
            skillEntity.AddAssetPath("Skill_1");
            var ownerDir = owner.GetFaceDirection().Dir.normalized;
            var pos = owner.GetWorldPos().Pos + ownerDir * 0.5f;
            skillEntity.AddWorldPos(pos);
            skillEntity.AddMoveDirection(ownerDir);
            skillEntity.AddMoveSpeed(4);
            skillEntity.AddDestroyCountdown(2.0f);
            skillEntity.AddFaceDirection();
            skillEntity.AddLocalScale(Vector3.one * 0.2f);
            skillEntity.AddTargetPos(owner.GetTargetEntity().Target.GetWorldPos().Pos);
            owner.AddAtkIntervalComponent(4.0f);
            owner.SetMoveSpeed(0);
            owner.AddLateSkillComponent(0.5f);
            EndAction(true);
        }
        

        protected override void OnUpdate()
        {
        }

        //Called when the task is disabled.
        protected override void OnStop()
        {
        }

        //Called when the task is paused.
        protected override void OnPause()
        {
        }
    }
}