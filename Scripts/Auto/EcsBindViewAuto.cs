   using GameFrame;
   using UnityEngine;
   public static class AutoView
   {
        
         public static void AddView(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent<GameFrame.View>();
         }
         
         public static void AddView(this ECSEntity ecsEntity,IEceView param)
         {
             var p  =  ecsEntity.AddComponent<GameFrame.View>();
             p.Value = param;
         }
         
         public static GameFrame.View GetView(this ECSEntity ecsEntity)
         {
              return ecsEntity.GetComponent<GameFrame.View>();
         }
         
         public static ECSEntity SetView(this ECSEntity ecsEntity,IEceView param)
         {
              var p = ecsEntity.GetComponent<GameFrame.View>();
              p.Value = param;
              
              return ecsEntity;
         }
              
   }