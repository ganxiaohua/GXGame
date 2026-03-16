using System.Collections.Generic;

namespace GamePlay.Runtime
{
    public class DataManager : Singleton<DataManager>
    {
        private List<IGameData> datalList = new List<IGameData>();

        public void Initialization()
        {
            datalList.Add(BagData.Instance);
            datalList.Add(LandData.Instance);
            datalList.Add(TimeData.Instance);
            datalList.Add(PlayerAttrData.Instance);
            datalList.Add(MapResData.Instance);
            foreach (var data in datalList)
            {
                data.Initialization();
            }
        }

        public void ShutDown()
        {
            foreach (var data in datalList)
            {
                data.ShutDown();
            }
        }

        public void RefreshDay()
        {
            EventSend.Instance.FireWorldEvent(WorldEventMsg.DaySettlement, null);
            TimeData.Instance.AddOneDay();
            LandData.Instance.SetAllSprinkleWater(false);
            MapResData.Instance.RefreshDay();
            EventSend.Instance.FireWorldEvent(WorldEventMsg.RefreshDate, null);
        }
    }
}