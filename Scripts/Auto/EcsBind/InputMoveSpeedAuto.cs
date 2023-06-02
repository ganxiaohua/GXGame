   using GameFrame;
   using UnityEngine;
   public static class AutoInputMoveSpeed
   {
        
         public static void AddInputMoveSpeed(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.InputMoveSpeed);
         }
         
         public static void AddInputMoveSpeed(this ECSEntity ecsEntity,System.Single param)
         {
             var p  =  (GXGame.InputMoveSpeed)ecsEntity.AddComponent(Components.InputMoveSpeed);
             p.MoveSpeed = param;
         }
         
         public static GXGame.InputMoveSpeed GetInputMoveSpeed(this ECSEntity ecsEntity)
         {
              return (GXGame.InputMoveSpeed)ecsEntity.GetComponent(Components.InputMoveSpeed);
         }
         
         public static ECSEntity SetInputMoveSpeed(this ECSEntity ecsEntity,System.Single param)
         {
              var p = (GXGame.InputMoveSpeed)ecsEntity.GetComponent(Components.InputMoveSpeed);
              p.MoveSpeed = param;
              
              return ecsEntity;
         }
              
   }