   using GameFrame;
   using UnityEngine;
   public static class AutoInputDirection
   {
        
        public static void AddInputDirection(this ECSEntity ecsEntity)
        {
            ecsEntity.AddComponent(Components.InputDirection);
        }        
        
        public static GXGame.InputDirection GetInputDirection(this ECSEntity ecsEntity)
        {
            return (GXGame.InputDirection)ecsEntity.GetComponent(Components.InputDirection);
        }     
   }