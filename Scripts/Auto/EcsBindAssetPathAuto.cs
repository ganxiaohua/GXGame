   using GameFrame;
   using UnityEngine;
   public static class AutoAssetPath
   {
        
         public static void AddAssetPath(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent<GXGame.AssetPath>();
         }
         
         public static void AddAssetPath(this ECSEntity ecsEntity,string param)
         {
             var p  =  ecsEntity.AddComponent<GXGame.AssetPath>();
             p.Path = param;
         }
         
         public static GXGame.AssetPath GetAssetPath(this ECSEntity ecsEntity)
         {
              return ecsEntity.GetComponent<GXGame.AssetPath>();
         }
         
         public static ECSEntity SetAssetPath(this ECSEntity ecsEntity,string param)
         {
              var p = ecsEntity.GetComponent<GXGame.AssetPath>();
              p.Path = param;
              
              return ecsEntity;
         }
              
   }