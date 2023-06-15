public static class Components {
    
    public const int WorldPos= 0;
    public const int WorldOffsetPos= 1;
    public const int WorldRotate= 2;
    public const int LocalScale= 3;
    public const int MeshRendererColor= 4;
    public const int InputDirection= 5;
    public const int InputMoveSpeed= 6;
    public const int AssetPath= 7;
    public const int SkillAbilityBehavior= 8;
    public const int SkillAbilityBehaviorEvent= 9;
    public const int SkillGroupComponent= 10;
    public const int SkillIDComponent= 11;
    public const int SkillOwnerComponent= 12;
    public const int Destroy= 13;
    public const int View= 14;
    
    public const int TotalComponents = 15;
    
    
    public static readonly System.Type[] ComponentTypes = {
        
        typeof(GXGame.WorldPos),
        typeof(GXGame.WorldOffsetPos),
        typeof(GXGame.WorldRotate),
        typeof(GXGame.LocalScale),
        typeof(GXGame.MeshRendererColor),
        typeof(GXGame.InputDirection),
        typeof(GXGame.InputMoveSpeed),
        typeof(GXGame.AssetPath),
        typeof(GXGame.SkillAbilityBehavior),
        typeof(GXGame.SkillAbilityBehaviorEvent),
        typeof(GXGame.SkillGroupComponent),
        typeof(GXGame.SkillIDComponent),
        typeof(GXGame.SkillOwnerComponent),
        typeof(GameFrame.Destroy),
        typeof(GameFrame.View),
    };

}
