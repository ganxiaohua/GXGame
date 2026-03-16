using System;

namespace GamePlay.Runtime
{
    [Flags]
    public enum CampType
    {
        None = 1 << 0,
        Player = 1 << 1,
        Monster = 1 << 2,
        Res = 1 << 3,
        Pet = 1 << 4,
        CropLand = 1 << 5,
        Crop = 1 << 6,
        Portal = 1 << 7,
    }
}