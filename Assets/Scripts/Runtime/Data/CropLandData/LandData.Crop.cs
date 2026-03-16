using System;

namespace GamePlay.Runtime
{
    public partial class LandData
    {
        public class CropData : IDisposable
        {
            public int LandId;
            public int Index;
            public int UnitId;
            public int Year;
            public int Moon;
            public int Day;
            public int PersistentDay;

            public void Dispose()
            {
            }
        }
    }
}