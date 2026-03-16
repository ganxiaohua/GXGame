using System;
using UnityEngine;

namespace GamePlay.Runtime
{
    // 以1现实分钟=1游戏小时进行时间换算
    //
    //  即一昼夜为24分钟
    //
    // 其中暂定6:00-18:00为白天
    public partial class TimeData : Singleton<TimeData>, IGameData
    {
        /// <summary>
        /// 单位游戏中的事件分
        /// </summary>
        private float curShowDayTime;

        private float curDayTime;

        public void Initialization()
        {
        }

        public void ShutDown()
        {
        }

        public void SetTime()
        {
            curDayTime = Time.realtimeSinceStartup;
            curShowDayTime = ConstData.OneDayStartTime * ConstData.OneHourMin;
            SetYearMoonDay();
        }

        /// <summary>
        /// 得到游戏世界今天的秒数。
        /// </summary>
        /// <returns></returns>
        public float GetCurTime()
        {
            float time = Time.realtimeSinceStartup - curDayTime;
            return time + curShowDayTime;
        }

        /// <summary>
        /// 刷新一天
        /// </summary>
        public void AddOneDay()
        {
            var allday = GetCurDayNumberWithYear() + 1;
            var yearDay = ConstData.OneMoonDay * ConstData.OneYearMoon;
            var year = allday / yearDay;
            int thisYearDay = allday - year * yearDay;
            int moon = thisYearDay / ConstData.OneMoonDay;
            int day = thisYearDay % ConstData.OneMoonDay;
            CurYear = year;
            CurMoon = moon;
            CurDay = day;
        }

        public string GetTimeString()
        {
            var time = GetCurTime();
            int hour = (int) (time / ConstData.OneHourMin);
            int min = (int) (time % ConstData.OneHourMin);
            return $"{CurMoon}Moon {CurDay}Day\n{hour}:{min:D2}";
        }
    }
}