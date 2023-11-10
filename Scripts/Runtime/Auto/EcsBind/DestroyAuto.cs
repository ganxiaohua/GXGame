   using GameFrame;
   using UnityEngine;
   public static class AutoDestroy
   {
        
         public static void AddDestroy(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.Destroy);
         }
         
         public static GameFrame.Destroy GetDestroy(this ECSEntity ecsEntity)
         {
              return (GameFrame.Destroy)ecsEntity.GetComponent(Components.Destroy);
         }
              
   }