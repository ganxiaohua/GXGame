   using GameFrame;
   using UnityEngine;
   public static class AutoSkillOwnerComponent
   {
        
         public static void AddSkillOwnerComponent(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.SkillOwnerComponent);
         }
         
         public static void AddSkillOwnerComponent(this ECSEntity ecsEntity,GameFrame.ECSEntity param)
         {
             var p  =  (GXGame.SkillOwnerComponent)ecsEntity.AddComponent(Components.SkillOwnerComponent);
             p.Owner = param;
         }
         
         public static GXGame.SkillOwnerComponent GetSkillOwnerComponent(this ECSEntity ecsEntity)
         {
              return (GXGame.SkillOwnerComponent)ecsEntity.GetComponent(Components.SkillOwnerComponent);
         }
         
         public static ECSEntity SetSkillOwnerComponent(this ECSEntity ecsEntity,GameFrame.ECSEntity param)
         {
              var p = (GXGame.SkillOwnerComponent)ecsEntity.GetComponent(Components.SkillOwnerComponent);
              p.Owner = param;
              
              return ecsEntity;
         }
              
   }