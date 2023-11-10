   using GameFrame;
   using UnityEngine;
   public static class AutoSkillModelTargetComponent
   {
        
         public static void AddSkillModelTargetComponent(this GXGame.SkillEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillModelTargetComponent);
         }
         
         public static void AddSkillModelTargetComponent(this GXGame.SkillEntity ecsEntity,GXGame.SkillTargetEnum[] param)
         {
             var p  =  (GXGame.SkillModelTargetComponent)ecsEntity.AddComponent(Components.SkillModelTargetComponent);
             p.SkillTargetEnum = param;
         }
         
         public static GXGame.SkillModelTargetComponent GetSkillModelTargetComponent(this GXGame.SkillEntity ecsEntity)
         {
              return (GXGame.SkillModelTargetComponent)ecsEntity.GetComponent(Components.SkillModelTargetComponent);
         }
         
         public static ECSEntity SetSkillModelTargetComponent(this GXGame.SkillEntity ecsEntity,GXGame.SkillTargetEnum[] param)
         {
              var p = (GXGame.SkillModelTargetComponent)ecsEntity.GetComponent(Components.SkillModelTargetComponent);
              p.SkillTargetEnum = param;
              
              return ecsEntity;
         }
              
   }