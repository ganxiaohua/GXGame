   using GameFrame;
   using UnityEngine;
   public static class AutoAbilityCastRangeComponent
   {
        
         public static void AddAbilityCastRangeComponent(this GXGame.SkillManagerEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.AbilityCastRangeComponent);
         }
         
         public static void AddAbilityCastRangeComponent(this GXGame.SkillManagerEntity ecsEntity,System.Single param)
         {
             var p  =  (GXGame.AbilityCastRangeComponent)ecsEntity.AddComponent(Components.AbilityCastRangeComponent);
             p.AbilityCastRange = param;
         }
         
         public static GXGame.AbilityCastRangeComponent GetAbilityCastRangeComponent(this GXGame.SkillManagerEntity ecsEntity)
         {
              return (GXGame.AbilityCastRangeComponent)ecsEntity.GetComponent(Components.AbilityCastRangeComponent);
         }
         
         public static ECSEntity SetAbilityCastRangeComponent(this GXGame.SkillManagerEntity ecsEntity,System.Single param)
         {
              var p = (GXGame.AbilityCastRangeComponent)ecsEntity.GetComponent(Components.AbilityCastRangeComponent);
              p.AbilityCastRange = param;
              
              return ecsEntity;
         }
              
   }