   using GameFrame;
   using UnityEngine;
   public static class AutoAssetPath
   {
        
         public static void AddAssetPath(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.AssetPath);
         }
         
         public static void AddAssetPath(this ECSEntity ecsEntity,System.String param)
         {
             var p  =  (GXGame.AssetPath)ecsEntity.AddComponent(Components.AssetPath);
             p.Path = param;
         }
         
         public static GXGame.AssetPath GetAssetPath(this ECSEntity ecsEntity)
         {
              return (GXGame.AssetPath)ecsEntity.GetComponent(Components.AssetPath);
         }
         
         public static ECSEntity SetAssetPath(this ECSEntity ecsEntity,System.String param)
         {
              var p = (GXGame.AssetPath)ecsEntity.GetComponent(Components.AssetPath);
              p.Path = param;
              
              return ecsEntity;
         }
              
   }