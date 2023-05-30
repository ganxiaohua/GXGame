   using GameFrame;
   using UnityEngine;
   public static class AutoDestroy
   {
        
         public static void AddDestroy(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent<GameFrame.Destroy>();
         }
         
         public static GameFrame.Destroy GetDestroy(this ECSEntity ecsEntity)
         {
              return ecsEntity.GetComponent<GameFrame.Destroy>();
         }
              
   }