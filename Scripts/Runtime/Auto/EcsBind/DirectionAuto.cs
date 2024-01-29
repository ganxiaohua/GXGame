   using GameFrame;
   using UnityEngine;
   public static class AutoDirection
   {
        
        public static void AddDirection(this ECSEntity ecsEntity)
        {
            ecsEntity.AddComponent(Components.Direction);
        }
         public static void AddDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
             var p  =  (GXGame.Direction)ecsEntity.AddComponent(Components.Direction);
             p.Dir = param;
         }
                 
        
        public static GXGame.Direction GetDirection(this ECSEntity ecsEntity)
        {
            return (GXGame.Direction)ecsEntity.GetComponent(Components.Direction);
        }        
        
        public static ECSEntity SetDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
        {
            var p = (GXGame.Direction)ecsEntity.GetComponent(Components.Direction);
            p.Dir = param;
            ViewBindEventClass.DirectionEntityComponentNumericalChange?.Invoke(p,ecsEntity);
            ((Context)ecsEntity.Parent).Reactive(Components.Direction, ecsEntity);
            return ecsEntity;
         }     
   }