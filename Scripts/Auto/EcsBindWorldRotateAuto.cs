   using GameFrame;
   using UnityEngine;
   public static class AutoWorldRotate
   {
        
         public static void AddWorldRotate(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent<GXGame.WorldRotate>();
         }
         
         public static void AddWorldRotate(this ECSEntity ecsEntity,Vector3 param)
         {
             var p  =  ecsEntity.AddComponent<GXGame.WorldRotate>();
             p.Rotate = param;
         }
         
         public static GXGame.WorldRotate GetWorldRotate(this ECSEntity ecsEntity)
         {
              return ecsEntity.GetComponent<GXGame.WorldRotate>();
         }
         
         public static ECSEntity SetWorldRotate(this ECSEntity ecsEntity,Vector3 param)
         {
              var p = ecsEntity.GetComponent<GXGame.WorldRotate>();
              p.Rotate = param;
              ViewBindEventClass.WorldRotateEntityComponentNumericalChange?.Invoke(p,ecsEntity);
              return ecsEntity;
         }
              
   }