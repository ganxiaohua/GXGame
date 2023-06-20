using GameFrame;

namespace  GXGame
{
    public class SkillGroupComponent : ECSComponent
    {
        public int[] IDs;
    }   
    
    [AssignBind(typeof(SkillManagerEntity))]
    public class SkillIDComponent : ECSComponent
    {
        public int ID;
    }   
    
    [AssignBind(typeof(SkillManagerEntity))]
    public class SkillManagerStateComponent : ECSComponent
    {
        public SkillManagerState SkillManagerState;
    }   
    
}