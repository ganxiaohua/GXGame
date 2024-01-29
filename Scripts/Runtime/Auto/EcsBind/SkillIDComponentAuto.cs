   using GameFrame;
   using UnityEngine;
   public static class AutoSkillIDComponent
   {
        
        public static void AddSkillIDComponent(this GXGame.SkillManagerEntity ecsEntity)
        {
            ecsEntity.AddComponent(Components.SkillIDComponent);
        }
         public static void AddSkillIDComponent(this GXGame.SkillManagerEntity ecsEntity,System.Int32 param)
         {
             var p  =  (GXGame.SkillIDComponent)ecsEntity.AddComponent(Components.SkillIDComponent);
             p.ID = param;
         }
                 
        
        public static GXGame.SkillIDComponent GetSkillIDComponent(this GXGame.SkillManagerEntity ecsEntity)
        {
            return (GXGame.SkillIDComponent)ecsEntity.GetComponent(Components.SkillIDComponent);
        }        
        
        public static ECSEntity SetSkillIDComponent(this GXGame.SkillManagerEntity ecsEntity,System.Int32 param)
        {
            var p = (GXGame.SkillIDComponent)ecsEntity.GetComponent(Components.SkillIDComponent);
            p.ID = param;
            
            ((Context)ecsEntity.Parent).Reactive(Components.SkillIDComponent, ecsEntity);
            return ecsEntity;
         }     
   }