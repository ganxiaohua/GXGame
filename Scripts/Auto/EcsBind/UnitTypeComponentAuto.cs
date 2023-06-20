   using GameFrame;
   using UnityEngine;
   public static class AutoUnitTypeComponent
   {
        
         public static void AddUnitTypeComponent(this ECSEntity ecsEntity)
         {
              ecsEntity.AddComponent(Components.UnitTypeComponent);
         }
         
         public static void AddUnitTypeComponent(this ECSEntity ecsEntity,GXGame.UnitTypeEnum param)
         {
             var p  =  (GXGame.UnitTypeComponent)ecsEntity.AddComponent(Components.UnitTypeComponent);
             p.UnitTypeEnum = param;
         }
         
         public static GXGame.UnitTypeComponent GetUnitTypeComponent(this ECSEntity ecsEntity)
         {
              return (GXGame.UnitTypeComponent)ecsEntity.GetComponent(Components.UnitTypeComponent);
         }
         
         public static ECSEntity SetUnitTypeComponent(this ECSEntity ecsEntity,GXGame.UnitTypeEnum param)
         {
              var p = (GXGame.UnitTypeComponent)ecsEntity.GetComponent(Components.UnitTypeComponent);
              p.UnitTypeEnum = param;
              
              return ecsEntity;
         }
              
   }