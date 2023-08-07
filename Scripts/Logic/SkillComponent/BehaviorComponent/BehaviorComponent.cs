using GameFrame;
using UnityEngine;

namespace GXGame
{
    
    [AssignBind(typeof(SkillEntity))]
    public class SkillComponent : ECSComponent
    {
    }
    
    [AssignBind(typeof(SkillEntity))]
    public class SkillSoundPathComponent : ECSComponent
    {
        public string[] Path;
    }
    
    [AssignBind(typeof(SkillEntity))]
    public class SkillSoundTargetComponent : ECSComponent
    {
        public SkillTargetEnum[] SkillTargetEnum;
    }

    [AssignBind(typeof(SkillEntity))]
    public class SkillPlayModelPathComponent : ECSComponent
    {
        public string[] AnimtionName;
    }
    
    [AssignBind(typeof(SkillEntity))]
    public class SkillModelTargetComponent : ECSComponent
    {
        public SkillTargetEnum[] SkillTargetEnum;
    }

    [AssignBind(typeof(SkillEntity))]
    public class SkillEffectComponent : ECSComponent
    {
        public string[] Path;
    }
    
    [AssignBind(typeof(SkillEntity))]
    public class SkillEffectEnitiyComponent : ECSComponent
    {
        public bool HasEffect;
    }
    
    [AssignBind(typeof(SkillEffectEntity))]
    public class SkillIsEffectComponent : ECSComponent
    {
        
    }


    [AssignBind(typeof(SkillEntity))]
    public class SkillEffectTargetComponent : ECSComponent
    {
        public SkillTargetEnum[] SkillTargetEnum;
    }

}