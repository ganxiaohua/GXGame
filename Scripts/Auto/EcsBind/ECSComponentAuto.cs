   using GameFrame;
   using UnityEngine;
   public static class AutoECSComponent
   {
        
         public static void AddECSComponent(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.ECSComponent);
         }
         
         public static GameFrame.ECSComponent GetECSComponent(this ECSEntity ecsEntity)
         {
              return (GameFrame.ECSComponent)ecsEntity.GetComponent(Components.ECSComponent);
         }
              
   }