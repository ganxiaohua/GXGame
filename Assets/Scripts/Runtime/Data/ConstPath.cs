namespace GamePlay.Runtime
{
    public static class ConstPath
    {
        public static string[] EffectsQuality = new string[]
        {
                "Assets/Res/Prefabs/Effects/Quality/ItemEpicModular",
                "Assets/Res/Prefabs/Effects/Quality/ItemEpicModular",
                "Assets/Res/Prefabs/Effects/Quality/ItemEpicGreen",
                "Assets/Res/Prefabs/Effects/Quality/ItemEpicOrange",
                "Assets/Res/Prefabs/Effects/Quality/ItemEpicOrange",
                "Assets/Res/Prefabs/Effects/Quality/ItemEpicOrange"
        };

        public static string PrefabsHouse = "Assets/Res/Prefabs/House";
        public static string MapPath = "Assets/Res/Prefabs/Map/Map_{0}/Map_{0}.prefab";

        public static string MapDataPath = "Assets/Res/Prefabs/Map/Map_{0}/MapArea_{0}.asset";

        public static string MapChunkPath = "Assets/Res/Prefabs/Map/Map_{0}/MapChunk_{1}.asset";

        public static string MapAstarAssetPath = "Assets/Res/Prefabs/Map/Map_{0}/AStarPath/{1}.bytes";

        public static string HousePath = "Assets/Res/Prefabs/House/House_{0}/House_{0}.prefab";
        public static string HouseAssetPath = "Assets/Res/Prefabs/House/House_{0}/HouseData_{0}.asset";

        public static string MapCropLandFold = "Assets/Res/Prefabs/Map/Map_{0}/CropLand";
        public static string MapCropLandPath = "Assets/Res/Prefabs/Map/Map_{0}/CropLand/CropLand_{1}.asset";
    }
}