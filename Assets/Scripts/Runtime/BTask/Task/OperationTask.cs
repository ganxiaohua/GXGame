using GameFrame.Runtime;
using NodeCanvas.Framework;
using ParadoxNotion.Design;

namespace GamePlay.Runtime
{
    [Category("AI")]
    public class OperationTask : ActionTask
    {
        private EffEntity owner;
        private BehaviorTableItem behaviorTableItem;
        // [BlackboardOnly] public BBParameter<EffEntity> Tagert;

        protected override string OnInit()
        {
            owner = blackboard.propertiesBindTarget.GetComponent<ViewEffBindEnitiy>().Entity;
            behaviorTableItem = owner.GetUnit().BehaviorTable_Ref;
            return null;
        }

        protected override void OnExecute()
        {
            owner.AddOrSetOperatedCountdownComp(behaviorTableItem.AtkAnimation);
            EndAction(true);
        }
    }
}