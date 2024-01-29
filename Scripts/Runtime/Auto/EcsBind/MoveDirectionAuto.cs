   using GameFrame;
   using UnityEngine;
   public static class AutoMoveDirection
   {
        
        public static void AddMoveDirection(this ECSEntity ecsEntity)
        {
            ecsEntity.AddComponent(Components.MoveDirection);
        }
         public static void AddMoveDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
         {
             var p  =  (GXGame.MoveDirection)ecsEntity.AddComponent(Components.MoveDirection);
             p.Dir = param;
         }
                 
        
        public static GXGame.MoveDirection GetMoveDirection(this ECSEntity ecsEntity)
        {
            return (GXGame.MoveDirection)ecsEntity.GetComponent(Components.MoveDirection);
        }        
        
        public static ECSEntity SetMoveDirection(this ECSEntity ecsEntity,UnityEngine.Vector3 param)
        {
            var p = (GXGame.MoveDirection)ecsEntity.GetComponent(Components.MoveDirection);
            p.Dir = param;
            
            ((Context)ecsEntity.Parent).Reactive(Components.MoveDirection, ecsEntity);
            return ecsEntity;
         }     
   }