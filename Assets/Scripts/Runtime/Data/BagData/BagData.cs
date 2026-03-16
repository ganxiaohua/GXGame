using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class ItemInfo : BulkListIndex
    {
        public Item Item;
        public int Count;

        public override void Dispose()
        {
            base.Dispose();
            Item = null;
            Count = 0;
        }
    }

    public partial class BagData : Singleton<BagData>, IGameData
    {
        public enum BagType
        {
            Pocket,
            Bag,
        }

        public void Initialization()
        {
            CurBagIndex = 0;
            PocketItems = new(ConstData.PocketSize);
            BagItems = new(ConstData.BagSize);
        }

        public void ShutDown()
        {
        }


        /// <summary>
        /// 只管加，不管加到哪个背包
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool AddItem(int id, int count)
        {
            int remaining = PocketReturnRemaining(id, count);
            int bagRemaining = BagReturnRemaining(id, remaining);
            if (bagRemaining > 0)
                return false;
            AddPocketItemWithId(id, count - remaining);
            AddBagItemWithId(id, remaining);
            EventSend.Instance.FireUIEvent(GamePlay.Runtime.UIEventMsg.IRefreshBag, null);
            HintObjectSystem.Instance.Set(id, count);
            return true;
        }

        public int GetItemCount(int id)
        {
            var count = GetPocketItemCountWithId(id) + GetBagItemCountWithId(id);
            return count;
        }

        /// <summary>
        /// 只管删，不管删到哪个背包
        /// </summary>
        /// <param name="id"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool SubItem(int id, int count)
        {
            var pocketItemCount = GetPocketItemCountWithId(id);
            var bagItemCount = GetBagItemCountWithId(id);
            if ((pocketItemCount + bagItemCount) < count)
                return false;
            var pocketCount = Mathf.Min(count, pocketItemCount);
            SubPocketItemWithId(id, pocketCount);
            count -= pocketCount;
            if (count > 0)
            {
                SubBagItemWithId(id, count);
            }

            return true;
        }

        /// <summary>
        ///  删除道具
        /// </summary>
        /// <param name="bagType"></param>
        /// <param name="index"></param>
        /// <param name="count"> =表示全部删除</param>
        public void RemoveItemWithIndex(BagType bagType, int index)
        {
            if (bagType == BagType.Pocket)
            {
                var item = GetPocketItemWithIndex(index);
                if (item == null)
                    return;
                RemovePocketItemWithIndex(index);
            }
            else if (bagType == BagType.Bag)
            {
                var item = GetBagItemWithIndex(index);
                if (item == null)
                    return;
                RemoveBagItemWithIndex(index);
            }
        }

        /// <summary>
        /// 对两个东西进行交换，或者把东西放过去
        /// </summary>
        /// <param name="srcType"></param>
        /// <param name="srcIndex"></param>
        /// <param name="dstType"></param>
        /// <param name="dstIndex"></param>
        public void ExchangeProps(BagType srcType, int srcIndex, BagType dstType, int dstIndex)
        {
            if (srcType == dstType && srcIndex == dstIndex)
                return;
            int srcId = 0;
            int srcCount = 0;
            if (srcType == BagType.Pocket)
            {
                var item = GetPocketItemWithIndex(srcIndex);
                if (item != null)
                {
                    srcId = item.key;
                    srcCount = item.Count;
                    RemovePocketItemWithIndex(srcIndex);
                }
            }
            else if (srcType == BagType.Bag)
            {
                var item = GetBagItemWithIndex(srcIndex);
                if (item != null)
                {
                    srcId = item.key;
                    srcCount = item.Count;
                    RemoveBagItemWithIndex(srcIndex);
                }
            }

            int dstId = 0;
            int dstCount = 0;
            if (dstType == BagType.Pocket)
            {
                var item = GetPocketItemWithIndex(dstIndex);
                if (item != null)
                {
                    dstId = item.key;
                    dstCount = item.Count;
                    RemovePocketItemWithIndex(dstIndex);
                }
            }
            else if (dstType == BagType.Bag)
            {
                var item = GetBagItemWithIndex(dstIndex);
                if (item != null)
                {
                    dstId = item.key;
                    dstCount = item.Count;
                    RemoveBagItemWithIndex(dstIndex);
                }
            }

            if (srcId != 0)
            {
                if (dstType == BagType.Pocket)
                {
                    PocketItems.AddItemInfoWithIndex(dstIndex, srcId, srcCount);
                }
                else if (dstType == BagType.Bag)
                {
                    BagItems.AddItemInfoWithIndex(dstIndex, srcId, srcCount);
                }
            }

            if (dstId != 0)
            {
                if (srcType == BagType.Pocket)
                {
                    PocketItems.AddItemInfoWithIndex(srcIndex, dstId, dstCount);
                }
                else if (srcType == BagType.Bag)
                {
                    BagItems.AddItemInfoWithIndex(srcIndex, dstId, dstCount);
                }
            }
        }
    }
}