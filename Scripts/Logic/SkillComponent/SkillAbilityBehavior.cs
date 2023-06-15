using GameFrame;

namespace GXGame
{
    public enum AbilityBehavior : int
    {
        BEHAVIOR_PASSIVE = 0,           //这是一个被动技能
        BEHAVIOR_NO_TARGET = 1 <<1,     //不需要指定目标,按下按钮就释放
        BEHAVIOR_UNIT_TARGET = 1<<2,    //需要指定一个目标
        BEHAVIOR_DIRECTIONAL = 1<<3,     //朝着正前方发射
    }

    public class SkillAbilityBehavior : ECSComponent
    {
        public AbilityBehavior AbilityBehavior;
    }
    
    public enum AbilityBehaviorEvent : int
    {
        OnChannelFinish = 0,        //持续施法完成
        OnChannelInterrupted,   //当持续施法被中断
        OnChannelSucceeded ,     //当持续施法成功
        OnOwnerDied,                //当拥有者死亡
        OnSpellStart                //当技能施法开始
    }
    
    public class SkillAbilityBehaviorEvent : ECSComponent
    {
        public AbilityBehaviorEvent[] AbilityBehaviorEvent;
    }
}