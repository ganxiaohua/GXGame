   using GameFrame;
   using UnityEngine;
   public static class AutoSkillCollisionRadiusComponent
   {
        
         public static void AddSkillCollisionRadiusComponent(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillCollisionRadiusComponent);
         }
         
         public static void AddSkillCollisionRadiusComponent(this ECSEntity ecsEntity,System.Single param)
         {
             var p  =  (GXGame.SkillCollisionRadiusComponent)ecsEntity.AddComponent(Components.SkillCollisionRadiusComponent);
             p.Radius = param;
         }
         
         public static GXGame.SkillCollisionRadiusComponent GetSkillCollisionRadiusComponent(this ECSEntity ecsEntity)
         {
              return (GXGame.SkillCollisionRadiusComponent)ecsEntity.GetComponent(Components.SkillCollisionRadiusComponent);
         }
         
         public static ECSEntity SetSkillCollisionRadiusComponent(this ECSEntity ecsEntity,System.Single param)
         {
              var p = (GXGame.SkillCollisionRadiusComponent)ecsEntity.GetComponent(Components.SkillCollisionRadiusComponent);
              p.Radius = param;
              
              return ecsEntity;
         }
              
   }