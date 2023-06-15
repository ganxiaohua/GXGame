   using GameFrame;
   using UnityEngine;
   public static class AutoSkillIDComponent
   {
        
         public static void AddSkillIDComponent(this GXGame.SkillEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillIDComponent);
         }
         
         public static void AddSkillIDComponent(this GXGame.SkillEntity ecsEntity,System.Int32 param)
         {
             var p  =  (GXGame.SkillIDComponent)ecsEntity.AddComponent(Components.SkillIDComponent);
             p.ID = param;
         }
         
         public static GXGame.SkillIDComponent GetSkillIDComponent(this GXGame.SkillEntity ecsEntity)
         {
              return (GXGame.SkillIDComponent)ecsEntity.GetComponent(Components.SkillIDComponent);
         }
         
         public static ECSEntity SetSkillIDComponent(this GXGame.SkillEntity ecsEntity,System.Int32 param)
         {
              var p = (GXGame.SkillIDComponent)ecsEntity.GetComponent(Components.SkillIDComponent);
              p.ID = param;
              
              return ecsEntity;
         }
              
   }