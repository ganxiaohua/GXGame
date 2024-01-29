   using GameFrame;
   using UnityEngine;
   public static class AutoSkillEffectComponent
   {
        
        public static void AddSkillEffectComponent(this GXGame.SkillEntity ecsEntity)
        {
            ecsEntity.AddComponent(Components.SkillEffectComponent);
        }
         public static void AddSkillEffectComponent(this GXGame.SkillEntity ecsEntity,System.String[] param)
         {
             var p  =  (GXGame.SkillEffectComponent)ecsEntity.AddComponent(Components.SkillEffectComponent);
             p.Path = param;
         }
                 
        
        public static GXGame.SkillEffectComponent GetSkillEffectComponent(this GXGame.SkillEntity ecsEntity)
        {
            return (GXGame.SkillEffectComponent)ecsEntity.GetComponent(Components.SkillEffectComponent);
        }        
        
        public static ECSEntity SetSkillEffectComponent(this GXGame.SkillEntity ecsEntity,System.String[] param)
        {
            var p = (GXGame.SkillEffectComponent)ecsEntity.GetComponent(Components.SkillEffectComponent);
            p.Path = param;
            
            ((Context)ecsEntity.Parent).Reactive(Components.SkillEffectComponent, ecsEntity);
            return ecsEntity;
         }     
   }