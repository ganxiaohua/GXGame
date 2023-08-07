using GameFrame;

namespace GXGame
{
    public class SkillCollisionShapeComponent:ECSComponent
    {
        public CollisionShape CollisionShape;
    }
    
    
    public class SkillCollisionRadiusComponent:ECSComponent
    {
        public float Radius;
    }
}
