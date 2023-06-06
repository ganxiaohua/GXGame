   using GameFrame;
   using UnityEngine;
   public static class AutoCubeTest
   {
        
         public static void AddCubeTest(this GXGame.Cube ecsEntity)
         {
              ecsEntity.AddComponent(Components.CubeTest);
         }
         
         public static void AddCubeTest(this GXGame.Cube ecsEntity,System.String param)
         {
             var p  =  (GXGame.CubeTest)ecsEntity.AddComponent(Components.CubeTest);
             p.testcube = param;
         }
         
         public static GXGame.CubeTest GetCubeTest(this GXGame.Cube ecsEntity)
         {
              return (GXGame.CubeTest)ecsEntity.GetComponent(Components.CubeTest);
         }
         
         public static ECSEntity SetCubeTest(this GXGame.Cube ecsEntity,System.String param)
         {
              var p = (GXGame.CubeTest)ecsEntity.GetComponent(Components.CubeTest);
              p.testcube = param;
              
              return ecsEntity;
         }
              
   }