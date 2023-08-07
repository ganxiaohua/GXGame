namespace GXGame
{
    
    public enum SkillManagerState
    {
        None,
        Start,
        Ing,
        End,
    }
    
    public enum AbilityBehavior : int
    {
        BEHAVIOR_PASSIVE = 1, //这是一个被动技能
        BEHAVIOR_NO_TARGET = 1 << 1, //不需要指定目标,按下按钮就释放
        BEHAVIOR_UNIT_TARGET = 1 << 2, //需要指定一个目标
        BEHAVIOR_DIRECTIONAL = 1 << 3, //朝着正前方发射
    }
    
    public enum SkillTargetEnum
    {
        CASTER, //施法者
        TARGET, //目标
        POINT, //点
        ATTACKER, //攻击者
        UNIT, //单位
    }

    public enum CollisionShape
    {
        Box,
        Sphere
    }
}