   using GameFrame;
   using UnityEngine;
   public static class AutoWorldPos
   {
        
         public static void AddWorldPos(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent<GXGame.WorldPos>();
         }
         
         public static void AddWorldPos(this ECSEntity ecsEntity,Vector3 param)
         {
             var p  =  ecsEntity.AddComponent<GXGame.WorldPos>();
             p.Pos = param;
         }
         
         public static GXGame.WorldPos GetWorldPos(this ECSEntity ecsEntity)
         {
              return ecsEntity.GetComponent<GXGame.WorldPos>();
         }
         
         public static ECSEntity SetWorldPos(this ECSEntity ecsEntity,Vector3 param)
         {
              var p = ecsEntity.GetComponent<GXGame.WorldPos>();
              p.Pos = param;
              ViewBindEventClass.WorldPosEntityComponentNumericalChange?.Invoke(p,ecsEntity);
              return ecsEntity;
         }
              
   }