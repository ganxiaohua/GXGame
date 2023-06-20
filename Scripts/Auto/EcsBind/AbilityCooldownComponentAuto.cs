   using GameFrame;
   using UnityEngine;
   public static class AutoAbilityCooldownComponent
   {
        
         public static void AddAbilityCooldownComponent(this GXGame.SkillManagerEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.AbilityCooldownComponent);
         }
         
         public static void AddAbilityCooldownComponent(this GXGame.SkillManagerEntity ecsEntity,System.Single param)
         {
             var p  =  (GXGame.AbilityCooldownComponent)ecsEntity.AddComponent(Components.AbilityCooldownComponent);
             p.AbilityCooldown = param;
         }
         
         public static GXGame.AbilityCooldownComponent GetAbilityCooldownComponent(this GXGame.SkillManagerEntity ecsEntity)
         {
              return (GXGame.AbilityCooldownComponent)ecsEntity.GetComponent(Components.AbilityCooldownComponent);
         }
         
         public static ECSEntity SetAbilityCooldownComponent(this GXGame.SkillManagerEntity ecsEntity,System.Single param)
         {
              var p = (GXGame.AbilityCooldownComponent)ecsEntity.GetComponent(Components.AbilityCooldownComponent);
              p.AbilityCooldown = param;
              
              return ecsEntity;
         }
              
   }