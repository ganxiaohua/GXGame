   using GameFrame;
   using UnityEngine;
   public static class AutoLocalScale
   {
        
         public static void AddLocalScale(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent<GXGame.LocalScale>();
         }
         
         public static void AddLocalScale(this ECSEntity ecsEntity,Vector3 param)
         {
             var p  =  ecsEntity.AddComponent<GXGame.LocalScale>();
             p.Scale = param;
         }
         
         public static GXGame.LocalScale GetLocalScale(this ECSEntity ecsEntity)
         {
              return ecsEntity.GetComponent<GXGame.LocalScale>();
         }
         
         public static ECSEntity SetLocalScale(this ECSEntity ecsEntity,Vector3 param)
         {
              var p = ecsEntity.GetComponent<GXGame.LocalScale>();
              p.Scale = param;
              ViewBindEventClass.LocalScaleEntityComponentNumericalChange?.Invoke(p,ecsEntity);
              return ecsEntity;
         }
              
   }