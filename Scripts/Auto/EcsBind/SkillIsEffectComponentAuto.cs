   using GameFrame;
   using UnityEngine;
   public static class AutoSkillIsEffectComponent
   {
        
         public static void AddSkillIsEffectComponent(this GXGame.SkillEffectEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillIsEffectComponent);
         }
         
         public static GXGame.SkillIsEffectComponent GetSkillIsEffectComponent(this GXGame.SkillEffectEntity ecsEntity)
         {
              return (GXGame.SkillIsEffectComponent)ecsEntity.GetComponent(Components.SkillIsEffectComponent);
         }
              
   }