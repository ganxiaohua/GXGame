   using GameFrame;
   using UnityEngine;
   public static class AutoMoveSpeed
   {
        
         public static void AddMoveSpeed(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.MoveSpeed);
         }
         
         public static void AddMoveSpeed(this ECSEntity ecsEntity,System.Single param)
         {
             var p  =  (GXGame.MoveSpeed)ecsEntity.AddComponent(Components.MoveSpeed);
             p.Speed = param;
         }
         
         public static GXGame.MoveSpeed GetMoveSpeed(this ECSEntity ecsEntity)
         {
              return (GXGame.MoveSpeed)ecsEntity.GetComponent(Components.MoveSpeed);
         }
         
         public static ECSEntity SetMoveSpeed(this ECSEntity ecsEntity,System.Single param)
         {
              var p = (GXGame.MoveSpeed)ecsEntity.GetComponent(Components.MoveSpeed);
              p.Speed = param;
              
              return ecsEntity;
         }
              
   }