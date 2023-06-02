   using GameFrame;
   using UnityEngine;
   public static class AutoWorldOffsetPos
   {
        
         public static void AddWorldOffsetPos(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.WorldOffsetPos);
         }
         
         public static void AddWorldOffsetPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
             var p  =  (GXGame.WorldOffsetPos)ecsEntity.AddComponent(Components.WorldOffsetPos);
             p.OffsetPos = param;
         }
         
         public static GXGame.WorldOffsetPos GetWorldOffsetPos(this ECSEntity ecsEntity)
         {
              return (GXGame.WorldOffsetPos)ecsEntity.GetComponent(Components.WorldOffsetPos);
         }
         
         public static ECSEntity SetWorldOffsetPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
              var p = (GXGame.WorldOffsetPos)ecsEntity.GetComponent(Components.WorldOffsetPos);
              p.OffsetPos = param;
              ViewBindEventClass.WorldOffsetPosEntityComponentNumericalChange?.Invoke(p,ecsEntity);
              return ecsEntity;
         }
              
   }