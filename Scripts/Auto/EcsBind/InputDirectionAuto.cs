   using GameFrame;
   using UnityEngine;
   public static class AutoInputDirection
   {
        
         public static void AddInputDirection(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.InputDirection);
         }
         
         public static void AddInputDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
             var p  =  (GXGame.InputDirection)ecsEntity.AddComponent(Components.InputDirection);
             p.InputDir = param;
         }
         
         public static GXGame.InputDirection GetInputDirection(this ECSEntity ecsEntity)
         {
              return (GXGame.InputDirection)ecsEntity.GetComponent(Components.InputDirection);
         }
         
         public static ECSEntity SetInputDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
              var p = (GXGame.InputDirection)ecsEntity.GetComponent(Components.InputDirection);
              p.InputDir = param;
              
              return ecsEntity;
         }
              
   }