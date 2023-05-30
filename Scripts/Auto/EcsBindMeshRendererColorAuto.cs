   using GameFrame;
   using UnityEngine;
   public static class AutoMeshRendererColor
   {
        public static void AddMeshRendererColor(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent<GXGame.MeshRendererColor>();
         }
         
         public static void AddMeshRendererColor(this ECSEntity ecsEntity,Color param)
         {
             var p  =  ecsEntity.AddComponent<GXGame.MeshRendererColor>();
             p.Color = param;
         }
         
         public static GXGame.MeshRendererColor GetMeshRendererColor(this ECSEntity ecsEntity)
         {
              return ecsEntity.GetComponent<GXGame.MeshRendererColor>();
         }
         
         public static ECSEntity SetMeshRendererColor(this ECSEntity ecsEntity,Color param)
         {
              var p = ecsEntity.GetComponent<GXGame.MeshRendererColor>();
              p.Color = param;
              ViewBindEventClass.MeshRendererColorEntityComponentNumericalChange?.Invoke(p,ecsEntity);
              return ecsEntity;
         }
              
   }