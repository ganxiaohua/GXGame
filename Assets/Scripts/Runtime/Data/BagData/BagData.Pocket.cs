using System.Collections.Generic;
using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public partial class BagData
    {
        /// <summary>
        /// 可使用的一行
        /// </summary>
        public ItemInfoBulk PocketItems { get; private set; }

        public int CurBagIndex { get; private set; }


        public ItemInfo GetCurPocketInfo()
        {
            return PocketItems.GetItemInfoWithIndex(CurBagIndex);
        }

        public void SetCurPocketndex(int curIndex)
        {
            CurBagIndex = ((curIndex % PocketItems.Count) + PocketItems.Count) % PocketItems.Count;
        }


        public int GetCurAnimation()
        {
            var item = GetCurPocketInfo();
            if (item == null || item.Item.UesAnimation == null)
                return 1;
            return item.Item.UesAnimation.Value;
        }

        public int GetCurOptAnimatorQueueNum()
        {
            var item = GetCurPocketInfo();
            if (item == null)
                return Tables.Instance.AnimationTable[1].AnimationHashIndex;
            return item.Item.UesAnimation_Ref.AnimationHashIndex;
        }

        public int PocketReturnRemaining(int id, int count)
        {
            return PocketItems.ReturnRemaining(id, count);
        }


        public int GetPocketItemCountWithId(int id)
        {
            return PocketItems.GetCountWithId(id);
        }

        public ItemInfo GetPocketItemWithIndex(int index)
        {
            return PocketItems.GetItemInfoWithIndex(index);
        }

        public void RemovePocketItemWithIndex(int index)
        {
            PocketItems.RmoveItemInfoWithIndex(index);
        }

        public void SubPocketItemWithId(int id, int changeCount)
        {
            PocketItems.SubItemInfoWithId(id, changeCount);
            EventSend.Instance.FireUIEvent(GamePlay.Runtime.UIEventMsg.IRefreshBag, null);
        }

        public void AddPocketItemWithId(int id, int count)
        {
            PocketItems.AddItemInfoWithId(id, count);
        }
    }
}