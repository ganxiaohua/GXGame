   using GameFrame;
   using UnityEngine;
   public static class AutoSkillAbilityBehaviorEvent
   {
        
         public static void AddSkillAbilityBehaviorEvent(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillAbilityBehaviorEvent);
         }
         
         public static void AddSkillAbilityBehaviorEvent(this ECSEntity ecsEntity,GXGame.AbilityBehaviorEvent[] param)
         {
             var p  =  (GXGame.SkillAbilityBehaviorEvent)ecsEntity.AddComponent(Components.SkillAbilityBehaviorEvent);
             p.AbilityBehaviorEvent = param;
         }
         
         public static GXGame.SkillAbilityBehaviorEvent GetSkillAbilityBehaviorEvent(this ECSEntity ecsEntity)
         {
              return (GXGame.SkillAbilityBehaviorEvent)ecsEntity.GetComponent(Components.SkillAbilityBehaviorEvent);
         }
         
         public static ECSEntity SetSkillAbilityBehaviorEvent(this ECSEntity ecsEntity,GXGame.AbilityBehaviorEvent[] param)
         {
              var p = (GXGame.SkillAbilityBehaviorEvent)ecsEntity.GetComponent(Components.SkillAbilityBehaviorEvent);
              p.AbilityBehaviorEvent = param;
              
              return ecsEntity;
         }
              
   }