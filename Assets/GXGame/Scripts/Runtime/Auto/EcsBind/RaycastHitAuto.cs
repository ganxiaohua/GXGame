//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoRaycastHit
{
    
    public static void AddRaycastHit(this ECSEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.RaycastHit);
    }
    
    public static void AddRaycastHit(this ECSEntity ecsEntity,System.Collections.Generic.List<UnityEngine.RaycastHit2D> param)
    {
        var p  =  (GXGame.RaycastHit)ecsEntity.AddComponent(Components.RaycastHit);
        p.Value = param;
    }
          
    public static GXGame.RaycastHit GetRaycastHit(this ECSEntity ecsEntity)
    {
        return (GXGame.RaycastHit)ecsEntity.GetComponent(Components.RaycastHit);
    }
     
    public static ECSEntity SetRaycastHit(this ECSEntity ecsEntity,System.Collections.Generic.List<UnityEngine.RaycastHit2D> param)
    {
        var p = (GXGame.RaycastHit)ecsEntity.GetComponent(Components.RaycastHit);
        p.Value = param;
        ((World)ecsEntity.Parent).Reactive(Components.RaycastHit, ecsEntity,EcsChangeEventState.UpdateType);
        
        return ecsEntity;
    }
         
}