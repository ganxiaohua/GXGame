public static class Components {
    
    public const int WorldPos= 0;
    public const int WorldOffsetPos= 1;
    public const int WorldRotate= 2;
    public const int LocalScale= 3;
    public const int MeshRendererColor= 4;
    public const int InputDirection= 5;
    public const int InputMoveSpeed= 6;
    public const int AssetPath= 7;
    public const int CubeTest= 8;
    public const int ECSComponent= 9;
    public const int Destroy= 10;
    public const int View= 11;
    
    public const int TotalComponents = 12;
    
    
    public static readonly System.Type[] ComponentTypes = {
        
        typeof(GXGame.WorldPos),
        typeof(GXGame.WorldOffsetPos),
        typeof(GXGame.WorldRotate),
        typeof(GXGame.LocalScale),
        typeof(GXGame.MeshRendererColor),
        typeof(GXGame.InputDirection),
        typeof(GXGame.InputMoveSpeed),
        typeof(GXGame.AssetPath),
        typeof(GXGame.CubeTest),
        typeof(GameFrame.ECSComponent),
        typeof(GameFrame.Destroy),
        typeof(GameFrame.View),
    };

}
