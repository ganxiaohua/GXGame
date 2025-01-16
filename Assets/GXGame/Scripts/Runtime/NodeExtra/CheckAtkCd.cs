using GameFrame;
using NodeCanvas.Framework;
using ParadoxNotion.Design;


namespace GXGame {

	[Category("怪物AI")]
	[Description("攻击间隔")]
	public class CheckAtkCd : ConditionTask {

		private ECSEntity owner;
		private World world;
		protected override string OnInit(){
			owner = (ECSEntity) blackboard.parent.GetVariable("Entity").value;
			world = ((World) owner.Parent);
			return null;
		}

		//Called whenever the condition gets enabled.
		protected override void OnEnable() {
			
		}

		//Called whenever the condition gets disabled.
		protected override void OnDisable() {
			
		}

		//Called once per frame while the condition is active.
		//Return whether the condition is success or failure.
		protected override bool OnCheck()
		{
			return owner.GetAtkIntervalComponent() == null;
		}
	}
}