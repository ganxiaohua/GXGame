using System;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class PlayerAttrData : Singleton<PlayerAttrData>, IGameData
    {
        private AttrData baseAttr;
        private AttrData weaponAttr;

        public class ActiveValue
        {
            public float CurActiveValueChangeStartTime;
            public float SceRecover;
            public float SceConsume;
            public int Fatigue;
            public int MaxActiveValue;

            private int curActiveValue;

            public int CurActiveValue
            {
                get
                {
                    if (SceConsume != 0)
                    {
                        var interval = Time.realtimeSinceStartup - CurActiveValueChangeStartTime;
                        curActiveValue -= (Mathf.CeilToInt(interval * SceConsume));
                        curActiveValue = Mathf.Max(0, curActiveValue);
                    }
                    else if (curActiveValue == CurLimit)
                    {
                    }
                    else if (SceRecover != 0)
                    {
                        var interval = Time.realtimeSinceStartup - CurActiveValueChangeStartTime;
                        curActiveValue += (Mathf.CeilToInt(interval * SceRecover));
                        curActiveValue = Mathf.Min(CurLimit, curActiveValue);
                    }

                    CurActiveValueChangeStartTime = Time.realtimeSinceStartup;
                    return curActiveValue;
                }
                set
                {
                    curActiveValue = value;
                    curActiveValue = Mathf.Min(curActiveValue, CurLimit);
                    curActiveValue = Mathf.Max(0, curActiveValue);
                }
            }

            public int CurLimit => MaxActiveValue - Fatigue;
        }

        public ActiveValue activeValue { get; private set; }

        public bool HasActiveValue => activeValue.CurActiveValue != 0;

        public void Initialization()
        {
            activeValue = new ActiveValue();
            activeValue.MaxActiveValue = 500;
            activeValue.CurActiveValue = activeValue.CurLimit;
            activeValue.SceRecover = 20;
            activeValue.Fatigue = 0;
        }

        public void ShutDown()
        {
        }


        public void OperationFatigueComparison(OperatedType type)
        {
            if (type == OperatedType.锄 || type == OperatedType.镐 ||
                type == OperatedType.斧 || type == OperatedType.镰 ||
                type == OperatedType.洒水 || type == OperatedType.播种)
            {
                AddFatigue(2);
            }
        }

        public void AddFatigue(int fatigue)
        {
            activeValue.Fatigue += fatigue;
            NotifyActionValue();
        }

        /// <summary>
        /// 开始消耗体力。
        /// </summary>
        /// <param name="sceConsume"></param>
        private void RecoverableConsume(float sceConsume)
        {
            activeValue.CurActiveValueChangeStartTime = Time.realtimeSinceStartup;
            activeValue.SceConsume = sceConsume;
        }

        /// <summary>
        /// 停止消耗体力
        /// </summary>
        private void StopRecoverableConsume()
        {
            if (activeValue.SceConsume == 0)
                return;
            activeValue.CurActiveValueChangeStartTime = Time.realtimeSinceStartup;
            activeValue.SceConsume = 0;
        }

        public bool ChangeActiveValue(bool change, float sceConsume)
        {
            if (change)
            {
                RecoverableConsume(sceConsume);
            }
            else
            {
                StopRecoverableConsume();
            }

            if (!HasActiveValue)
                return false;
            return true;
        }

        public void NotifyActionValue()
        {
            EventSend.Instance.FireUIEvent(UIEventMsg.IRefreshActionValue, null);
        }
    }
}