using GameFrame;

namespace  GXGame
{
    public class SkillGroupComponent : ECSComponent
    {
        public int[] IDs;
    }   
    
    [AssignBind(typeof(SkillEntity))]
    public class SkillIDComponent : ECSComponent
    {
        public int ID;
    }   
}