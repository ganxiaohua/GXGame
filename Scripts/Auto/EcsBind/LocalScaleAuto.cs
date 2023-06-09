   using GameFrame;
   using UnityEngine;
   public static class AutoLocalScale
   {
        
         public static void AddLocalScale(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.LocalScale);
         }
         
         public static void AddLocalScale(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
             var p  =  (GXGame.LocalScale)ecsEntity.AddComponent(Components.LocalScale);
             p.Scale = param;
         }
         
         public static GXGame.LocalScale GetLocalScale(this ECSEntity ecsEntity)
         {
              return (GXGame.LocalScale)ecsEntity.GetComponent(Components.LocalScale);
         }
         
         public static ECSEntity SetLocalScale(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
              var p = (GXGame.LocalScale)ecsEntity.GetComponent(Components.LocalScale);
              p.Scale = param;
              ViewBindEventClass.LocalScaleEntityComponentNumericalChange?.Invoke(p,ecsEntity);
              return ecsEntity;
         }
              
   }