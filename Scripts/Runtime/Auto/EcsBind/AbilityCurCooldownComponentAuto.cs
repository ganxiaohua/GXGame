   using GameFrame;
   using UnityEngine;
   public static class AutoAbilityCurCooldownComponent
   {
        
         public static void AddAbilityCurCooldownComponent(this GXGame.SkillManagerEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.AbilityCurCooldownComponent);
         }
         
         public static void AddAbilityCurCooldownComponent(this GXGame.SkillManagerEntity ecsEntity,System.Single param)
         {
             var p  =  (GXGame.AbilityCurCooldownComponent)ecsEntity.AddComponent(Components.AbilityCurCooldownComponent);
             p.AbilityCurCooldown = param;
         }
         
         public static GXGame.AbilityCurCooldownComponent GetAbilityCurCooldownComponent(this GXGame.SkillManagerEntity ecsEntity)
         {
              return (GXGame.AbilityCurCooldownComponent)ecsEntity.GetComponent(Components.AbilityCurCooldownComponent);
         }
         
         public static ECSEntity SetAbilityCurCooldownComponent(this GXGame.SkillManagerEntity ecsEntity,System.Single param)
         {
              var p = (GXGame.AbilityCurCooldownComponent)ecsEntity.GetComponent(Components.AbilityCurCooldownComponent);
              p.AbilityCurCooldown = param;
              
              return ecsEntity;
         }
              
   }