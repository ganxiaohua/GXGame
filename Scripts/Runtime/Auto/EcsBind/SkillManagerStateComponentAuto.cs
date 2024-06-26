//------------------------------------------------------------------------------
// <auto-generated>
// </auto-generated>
//------------------------------------------------------------------------------
using GameFrame;
using UnityEngine;
public static class AutoSkillManagerStateComponent
{
    
    public static void AddSkillManagerStateComponent(this GXGame.SkillManagerEntity ecsEntity)
    {
        ecsEntity.AddComponent(Components.SkillManagerStateComponent);
    }
    
    public static void AddSkillManagerStateComponent(this GXGame.SkillManagerEntity ecsEntity,GXGame.SkillManagerState param)
    {
        var p  =  (GXGame.SkillManagerStateComponent)ecsEntity.AddComponent(Components.SkillManagerStateComponent);
        p.SkillManagerState = param;
    }
          
    public static GXGame.SkillManagerStateComponent GetSkillManagerStateComponent(this GXGame.SkillManagerEntity ecsEntity)
    {
        return (GXGame.SkillManagerStateComponent)ecsEntity.GetComponent(Components.SkillManagerStateComponent);
    }
     
    public static ECSEntity SetSkillManagerStateComponent(this GXGame.SkillManagerEntity ecsEntity,GXGame.SkillManagerState param)
    {
        var p = (GXGame.SkillManagerStateComponent)ecsEntity.GetComponent(Components.SkillManagerStateComponent);
        p.SkillManagerState = param;
        
        ((World)ecsEntity.Parent).Reactive(Components.SkillManagerStateComponent, ecsEntity);
        return ecsEntity;
    }
         
}