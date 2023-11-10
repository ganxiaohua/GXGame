using GameFrame;
using UnityEngine;

namespace GXGame
{
    /// <summary>
    /// 技能原点
    /// </summary>
    [AssignBind(typeof(SkillManagerEntity))]
    public class AbilityCastPointComponent : ECSComponent
    {
        public Vector3 AbilityCastPoint;
    }

    /// <summary>
    /// 技能范围
    /// </summary>
    [AssignBind(typeof(SkillManagerEntity))]
    public class AbilityCastRangeComponent : ECSComponent
    {
        public float AbilityCastRange;
    }

    /// <summary>
    /// 技能冷却时间
    /// </summary>
    [AssignBind(typeof(SkillManagerEntity))]
    public class AbilityCooldownComponent : ECSComponent
    {
        public float AbilityCooldown;
    }
    
    /// <summary>
    /// 技能当前冷却时间
    /// </summary>
    [AssignBind(typeof(SkillManagerEntity))]
    public class AbilityCurCooldownComponent : ECSComponent
    {
        public float AbilityCurCooldown;
    }
    
    /// <summary>
    /// 技能作用的阵营
    /// </summary>
    [AssignBind(typeof(SkillManagerEntity))]
    public class AbilityUnitTargetCampComponent : ECSComponent
    {
        public Camp AbilityUnitTargetTeam;
    }
    
    /// <summary>
    /// 技能作用的角色类型
    /// </summary>
    [AssignBind(typeof(SkillManagerEntity))]
    public class AbilityUnitTypeComponent : ECSComponent
    {
        public UnitTypeEnum AbilityUnitTargetTeam;
    }
}