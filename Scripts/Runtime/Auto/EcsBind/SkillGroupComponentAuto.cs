   using GameFrame;
   using UnityEngine;
   public static class AutoSkillGroupComponent
   {
        
        public static void AddSkillGroupComponent(this ECSEntity ecsEntity)
        {
            ecsEntity.AddComponent(Components.SkillGroupComponent);
        }
         public static void AddSkillGroupComponent(this ECSEntity ecsEntity,System.Int32[] param)
         {
             var p  =  (GXGame.SkillGroupComponent)ecsEntity.AddComponent(Components.SkillGroupComponent);
             p.IDs = param;
         }
                 
        
        public static GXGame.SkillGroupComponent GetSkillGroupComponent(this ECSEntity ecsEntity)
        {
            return (GXGame.SkillGroupComponent)ecsEntity.GetComponent(Components.SkillGroupComponent);
        }        
        
        public static ECSEntity SetSkillGroupComponent(this ECSEntity ecsEntity,System.Int32[] param)
        {
            var p = (GXGame.SkillGroupComponent)ecsEntity.GetComponent(Components.SkillGroupComponent);
            p.IDs = param;
            
            ((Context)ecsEntity.Parent).Reactive(Components.SkillGroupComponent, ecsEntity);
            return ecsEntity;
         }     
   }