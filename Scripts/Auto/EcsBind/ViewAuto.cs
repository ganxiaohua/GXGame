   using GameFrame;
   using UnityEngine;
   public static class AutoView
   {
        
         public static void AddView(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.View);
         }
         
         public static void AddView(this ECSEntity ecsEntity,GameFrame.IEceView param)
         {
             var p  =  (GameFrame.View)ecsEntity.AddComponent(Components.View);
             p.Value = param;
         }
         
         public static GameFrame.View GetView(this ECSEntity ecsEntity)
         {
              return (GameFrame.View)ecsEntity.GetComponent(Components.View);
         }
         
         public static ECSEntity SetView(this ECSEntity ecsEntity,GameFrame.IEceView param)
         {
              var p = (GameFrame.View)ecsEntity.GetComponent(Components.View);
              p.Value = param;
              
              return ecsEntity;
         }
              
   }