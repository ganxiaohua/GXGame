   using GameFrame;
   using UnityEngine;
   public static class AutoSkillCollisionShapeComponent
   {
        
         public static void AddSkillCollisionShapeComponent(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillCollisionShapeComponent);
         }
         
         public static void AddSkillCollisionShapeComponent(this ECSEntity ecsEntity,GXGame.CollisionShape param)
         {
             var p  =  (GXGame.SkillCollisionShapeComponent)ecsEntity.AddComponent(Components.SkillCollisionShapeComponent);
             p.CollisionShape = param;
         }
         
         public static GXGame.SkillCollisionShapeComponent GetSkillCollisionShapeComponent(this ECSEntity ecsEntity)
         {
              return (GXGame.SkillCollisionShapeComponent)ecsEntity.GetComponent(Components.SkillCollisionShapeComponent);
         }
         
         public static ECSEntity SetSkillCollisionShapeComponent(this ECSEntity ecsEntity,GXGame.CollisionShape param)
         {
              var p = (GXGame.SkillCollisionShapeComponent)ecsEntity.GetComponent(Components.SkillCollisionShapeComponent);
              p.CollisionShape = param;
              
              return ecsEntity;
         }
              
   }