   using GameFrame;
   using UnityEngine;
   public static class AutoDirectionSpeed
   {
        
         public static void AddDirectionSpeed(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.DirectionSpeed);
         }
         
         public static void AddDirectionSpeed(this ECSEntity ecsEntity,System.Single param)
         {
             var p  =  (GXGame.DirectionSpeed)ecsEntity.AddComponent(Components.DirectionSpeed);
             p.Speed = param;
         }
         
         public static GXGame.DirectionSpeed GetDirectionSpeed(this ECSEntity ecsEntity)
         {
              return (GXGame.DirectionSpeed)ecsEntity.GetComponent(Components.DirectionSpeed);
         }
         
         public static ECSEntity SetDirectionSpeed(this ECSEntity ecsEntity,System.Single param)
         {
              var p = (GXGame.DirectionSpeed)ecsEntity.GetComponent(Components.DirectionSpeed);
              p.Speed = param;
              
              return ecsEntity;
         }
              
   }