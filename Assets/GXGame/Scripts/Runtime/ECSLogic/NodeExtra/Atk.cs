using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion.Design;
using UnityEngine;


namespace GXGame
{
    [Category("怪物AI")]
    [Description("攻击")]
    public class Atk : ActionTask
    {
        private EffEntity owner;
        private World world;

        protected override string OnInit()
        {
            owner = (EffEntity) blackboard.parent.GetVariable("Entity").value;
            world = ((World) owner.Parent);
            return null;
        }

        protected override void OnExecute()
        {
            var skillEntity = world.AddChild<SkillEntity>();
            skillEntity.Name = "子弹";
            // skillEntity.AddSkillComponent(1);
            skillEntity.AddViewType(typeof(GoBaseView));
            skillEntity.AddAssetPath("Skill_1");
            var ownerDir = owner.GetFaceDirection().Value.normalized;
            Vector2 pos = owner.GetWorldPos().Value + ownerDir * 0.5f;
            skillEntity.AddWorldPos(pos);
            skillEntity.AddMoveDirection(owner.GetFaceDirection().Value);
            skillEntity.AddMoveSpeed(4);
            skillEntity.AddDestroyCountdown(2.0f);
            skillEntity.AddLocalScale(Vector3.one * 0.3f);
            skillEntity.AddCampComponent(GXGame.Camp.ENEMY);
            skillEntity.AddCollisionBox(CollisionBox.Create(skillEntity, LayerMask.NameToLayer($"Object")));
            skillEntity.AddCollisionGroundType(CollisionGroundType.Reflect);
            skillEntity.AddHP(1);
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