   using GameFrame;
   using UnityEngine;
   public static class AutoWorldPos
   {
        
        public static void AddWorldPos(this ECSEntity ecsEntity)
        {
            ecsEntity.AddComponent(Components.WorldPos);
        }
         public static void AddWorldPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
             var p  =  (GXGame.WorldPos)ecsEntity.AddComponent(Components.WorldPos);
             p.Pos = param;
         }
                 
        
        public static GXGame.WorldPos GetWorldPos(this ECSEntity ecsEntity)
        {
            return (GXGame.WorldPos)ecsEntity.GetComponent(Components.WorldPos);
        }        
        
        public static ECSEntity SetWorldPos(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
        {
            var p = (GXGame.WorldPos)ecsEntity.GetComponent(Components.WorldPos);
            p.Pos = param;
            ViewBindEventClass.WorldPosEntityComponentNumericalChange?.Invoke(p,ecsEntity);
            ((Context)ecsEntity.Parent).Reactive(Components.WorldPos, ecsEntity);
            return ecsEntity;
         }     
   }