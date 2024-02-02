using GameFrame;

namespace  GXGame
{
    public class SkillGroupComponent : ECSComponent
    {
        public int[] SkillIds;
    }   
    
    [AssignBind(typeof(SkillManagerEntity))]
    public class SkillIDComponent : ECSComponent
    {
        public int SkillID;
    }
    
    [AssignBind(typeof(SkillManagerEntity))]
    public class SkillManagerStateComponent : ECSComponent
    {
        public SkillManagerState SkillManagerState;
    }   
    
}