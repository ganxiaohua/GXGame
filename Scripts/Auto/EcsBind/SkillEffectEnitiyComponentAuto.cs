   using GameFrame;
   using UnityEngine;
   public static class AutoSkillEffectEnitiyComponent
   {
        
         public static void AddSkillEffectEnitiyComponent(this GXGame.SkillEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillEffectEnitiyComponent);
         }
         
         public static void AddSkillEffectEnitiyComponent(this GXGame.SkillEntity ecsEntity,GXGame.SkillEffectEntity param)
         {
             var p  =  (GXGame.SkillEffectEnitiyComponent)ecsEntity.AddComponent(Components.SkillEffectEnitiyComponent);
             p.Effect = param;
         }
         
         public static GXGame.SkillEffectEnitiyComponent GetSkillEffectEnitiyComponent(this GXGame.SkillEntity ecsEntity)
         {
              return (GXGame.SkillEffectEnitiyComponent)ecsEntity.GetComponent(Components.SkillEffectEnitiyComponent);
         }
         
         public static ECSEntity SetSkillEffectEnitiyComponent(this GXGame.SkillEntity ecsEntity,GXGame.SkillEffectEntity param)
         {
              var p = (GXGame.SkillEffectEnitiyComponent)ecsEntity.GetComponent(Components.SkillEffectEnitiyComponent);
              p.Effect = param;
              
              return ecsEntity;
         }
              
   }