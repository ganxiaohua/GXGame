using System;

namespace GamePlay.Runtime.Skill
{
    [Flags]
    public enum SkillBehavior
    {
        抛投技能 = 1 << 0,
        直线技能 = 1 << 1,
    }
}