   using GameFrame;
   using UnityEngine;
   public static class AutoSkillSoundTargetComponent
   {
        
         public static void AddSkillSoundTargetComponent(this GXGame.SkillEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillSoundTargetComponent);
         }
         
         public static void AddSkillSoundTargetComponent(this GXGame.SkillEntity ecsEntity,GXGame.SkillTargetEnum[] param)
         {
             var p  =  (GXGame.SkillSoundTargetComponent)ecsEntity.AddComponent(Components.SkillSoundTargetComponent);
             p.SkillTargetEnum = param;
         }
         
         public static GXGame.SkillSoundTargetComponent GetSkillSoundTargetComponent(this GXGame.SkillEntity ecsEntity)
         {
              return (GXGame.SkillSoundTargetComponent)ecsEntity.GetComponent(Components.SkillSoundTargetComponent);
         }
         
         public static ECSEntity SetSkillSoundTargetComponent(this GXGame.SkillEntity ecsEntity,GXGame.SkillTargetEnum[] param)
         {
              var p = (GXGame.SkillSoundTargetComponent)ecsEntity.GetComponent(Components.SkillSoundTargetComponent);
              p.SkillTargetEnum = param;
              
              return ecsEntity;
         }
              
   }