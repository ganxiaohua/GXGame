namespace GamePlay.Runtime
{
    public partial class TimeData
    {
        public int CurYear { get; private set; }
        public int CurMoon { get; private set; }
        public int CurDay { get; private set; }

        public void SetYearMoonDay()
        {
            CurYear = 0;
            CurMoon = 0;
            CurDay = 0;
        }

        // public void AddDay()
        // {
        //     CurDay++;
        // }
        //
        // public void AddMoon()
        // {
        //     CurMoon++;
        // }
        //
        // public void AddYear()
        // {
        //     CurYear++;
        // }

        /// <summary>
        /// 获取今年的第几天
        /// </summary>
        public int GetCurDayNumberWithYear()
        {
            int day = 0;
            for (int i = 0; i < CurMoon; i++)
            {
                day += ConstData.OneMoonDay;
            }

            return day + CurDay;
        }
    }
}