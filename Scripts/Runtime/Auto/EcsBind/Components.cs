public static class Components {
    
    public const int WorldPos= 0;
    public const int WorldRotate= 1;
    public const int LocalScale= 2;
    public const int MeshRendererColor= 3;
    public const int Direction= 4;
    public const int DirectionSpeed= 5;
    public const int InputDirection= 6;
    public const int MoveDirection= 7;
    public const int MoveSpeed= 8;
    public const int AssetPath= 9;
    public const int ViewType= 10;
    public const int CampComponent= 11;
    public const int UnitTypeComponent= 12;
    public const int SkillComponent= 13;
    public const int SkillSoundPathComponent= 14;
    public const int SkillSoundTargetComponent= 15;
    public const int SkillPlayModelPathComponent= 16;
    public const int SkillModelTargetComponent= 17;
    public const int SkillEffectComponent= 18;
    public const int SkillEffectEnitiyComponent= 19;
    public const int SkillIsEffectComponent= 20;
    public const int SkillEffectTargetComponent= 21;
    public const int AbilityCastPointComponent= 22;
    public const int AbilityCastRangeComponent= 23;
    public const int AbilityCooldownComponent= 24;
    public const int AbilityCurCooldownComponent= 25;
    public const int AbilityUnitTargetCampComponent= 26;
    public const int AbilityUnitTypeComponent= 27;
    public const int OnSpellStartComponent= 28;
    public const int SkillAbilityBehaviorComponent= 29;
    public const int SkillCollisionShapeComponent= 30;
    public const int SkillCollisionRadiusComponent= 31;
    public const int SkillGroupComponent= 32;
    public const int SkillIDComponent= 33;
    public const int SkillManagerStateComponent= 34;
    public const int SkillOwnerComponent= 35;
    public const int Destroy= 36;
    public const int View= 37;
    
    public const int TotalComponents = 38;
    
    
    public static readonly System.Type[] ComponentTypes = {
        typeof(GXGame.WorldPos),
        typeof(GXGame.WorldRotate),
        typeof(GXGame.LocalScale),
        typeof(GXGame.MeshRendererColor),
        typeof(GXGame.Direction),
        typeof(GXGame.DirectionSpeed),
        typeof(GXGame.InputDirection),
        typeof(GXGame.MoveDirection),
        typeof(GXGame.MoveSpeed),
        typeof(GXGame.AssetPath),
        typeof(GXGame.ViewType),
        typeof(GXGame.CampComponent),
        typeof(GXGame.UnitTypeComponent),
        typeof(GXGame.SkillComponent),
        typeof(GXGame.SkillSoundPathComponent),
        typeof(GXGame.SkillSoundTargetComponent),
        typeof(GXGame.SkillPlayModelPathComponent),
        typeof(GXGame.SkillModelTargetComponent),
        typeof(GXGame.SkillEffectComponent),
        typeof(GXGame.SkillEffectEnitiyComponent),
        typeof(GXGame.SkillIsEffectComponent),
        typeof(GXGame.SkillEffectTargetComponent),
        typeof(GXGame.AbilityCastPointComponent),
        typeof(GXGame.AbilityCastRangeComponent),
        typeof(GXGame.AbilityCooldownComponent),
        typeof(GXGame.AbilityCurCooldownComponent),
        typeof(GXGame.AbilityUnitTargetCampComponent),
        typeof(GXGame.AbilityUnitTypeComponent),
        typeof(GXGame.OnSpellStartComponent),
        typeof(GXGame.SkillAbilityBehaviorComponent),
        typeof(GXGame.SkillCollisionShapeComponent),
        typeof(GXGame.SkillCollisionRadiusComponent),
        typeof(GXGame.SkillGroupComponent),
        typeof(GXGame.SkillIDComponent),
        typeof(GXGame.SkillManagerStateComponent),
        typeof(GXGame.SkillOwnerComponent),
        typeof(GameFrame.Destroy),
        typeof(GameFrame.View),
    };

    public static void SetComponent()
    {
        GXComponents.ComponentTypes = ComponentTypes;   
    }
}
