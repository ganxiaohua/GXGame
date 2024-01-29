   using GameFrame;
   using UnityEngine;
   public static class AutoViewType
   {
        
        public static void AddViewType(this ECSEntity ecsEntity)
        {
            ecsEntity.AddComponent(Components.ViewType);
        }
         public static void AddViewType(this ECSEntity ecsEntity,System.Type param)
         {
             var p  =  (GXGame.ViewType)ecsEntity.AddComponent(Components.ViewType);
             p.Type = param;
         }
                 
        
        public static GXGame.ViewType GetViewType(this ECSEntity ecsEntity)
        {
            return (GXGame.ViewType)ecsEntity.GetComponent(Components.ViewType);
        }        
        
        public static ECSEntity SetViewType(this ECSEntity ecsEntity,System.Type param)
        {
            var p = (GXGame.ViewType)ecsEntity.GetComponent(Components.ViewType);
            p.Type = param;
            
            ((Context)ecsEntity.Parent).Reactive(Components.ViewType, ecsEntity);
            return ecsEntity;
         }     
   }