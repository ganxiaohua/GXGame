   using GameFrame;
   using UnityEngine;
   public static class AutoSkillAbilityBehavior
   {
        
         public static void AddSkillAbilityBehavior(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillAbilityBehavior);
         }
         
         public static void AddSkillAbilityBehavior(this ECSEntity ecsEntity,GXGame.AbilityBehavior param)
         {
             var p  =  (GXGame.SkillAbilityBehavior)ecsEntity.AddComponent(Components.SkillAbilityBehavior);
             p.AbilityBehavior = param;
         }
         
         public static GXGame.SkillAbilityBehavior GetSkillAbilityBehavior(this ECSEntity ecsEntity)
         {
              return (GXGame.SkillAbilityBehavior)ecsEntity.GetComponent(Components.SkillAbilityBehavior);
         }
         
         public static ECSEntity SetSkillAbilityBehavior(this ECSEntity ecsEntity,GXGame.AbilityBehavior param)
         {
              var p = (GXGame.SkillAbilityBehavior)ecsEntity.GetComponent(Components.SkillAbilityBehavior);
              p.AbilityBehavior = param;
              
              return ecsEntity;
         }
              
   }