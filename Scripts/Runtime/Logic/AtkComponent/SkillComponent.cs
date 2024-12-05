using GameFrame;

namespace GXGame
{
    /// <summary>
    /// 技能前摇
    /// </summary>
    public class PreSkillComponent : ECSComponent
    {
        public float Time;
    }

    /// <summary>
    /// 技能后摇
    /// </summary>
    public class LateSkillComponent : ECSComponent
    {
        public float Time;
    }

    /// <summary>
    /// 释放技能的间隔
    /// </summary>
    public class AtkIntervalComponent : ECSComponent
    {
        public float Time;
    }
    
    [AssignBind(typeof(SkillEntity))]
    public class SkillComponent : ECSComponent
    {
        public int ID;
    }
    
    /// <summary>
    /// 技能作用的角色类型
    /// </summary>
    [AssignBind(typeof(SkillEntity))]
    public class AbilityUnitTypeComponent : ECSComponent
    {
        public UnitTypeEnum AbilityUnitTargetTeam;
    }
}