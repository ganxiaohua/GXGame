using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace GXGame {

	[Category("怪物AI")]
	[Description("技能后摇")]
	public class LateSkill : ActionTask {
		
		private ECSEntity owner;
		private World world;
		protected override string OnInit(){
			owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
			world = ((World) owner.Parent);
			return null;
		}
		protected override void OnExecute()
		{
			if (owner.GetLateSkillComponent() == null)
			{
				owner.GetMoveSpeed().Speed = 0.5f;
				EndAction(true);
			}
			EndAction(false);
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