   using GameFrame;
   using UnityEngine;
   public static class AutoWorldRotate
   {
        
         public static void AddWorldRotate(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.WorldRotate);
         }
         
         public static void AddWorldRotate(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
             var p  =  (GXGame.WorldRotate)ecsEntity.AddComponent(Components.WorldRotate);
             p.Rotate = param;
         }
         
         public static GXGame.WorldRotate GetWorldRotate(this ECSEntity ecsEntity)
         {
              return (GXGame.WorldRotate)ecsEntity.GetComponent(Components.WorldRotate);
         }
         
         public static ECSEntity SetWorldRotate(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
              var p = (GXGame.WorldRotate)ecsEntity.GetComponent(Components.WorldRotate);
              p.Rotate = param;
              ViewBindEventClass.WorldRotateEntityComponentNumericalChange?.Invoke(p,ecsEntity);
              return ecsEntity;
         }
              
   }