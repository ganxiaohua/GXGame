using GameFrame.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class ItemInfoBulk
    {
        private BulkList<ItemInfo> itemInfos;

        public int Count => itemInfos.Count;

        public ItemInfoBulk(int count)
        {
            itemInfos = new BulkList<ItemInfo>(count);
        }

        public int GetCountWithId(int id)
        {
            int count = 0;
            var list = itemInfos.Get(id);
            if (list != null)
                for (int i = 0; i < list.Count; i++)
                {
                    count += list.Count;
                }

            return count;
        }

        public ItemInfo GetItemInfoWithIndex(int index)
        {
            Assert.IsTrue(index < itemInfos.Count, "array out {index}");
            return itemInfos[index];
        }

        public void RmoveItemInfoWithIndex(int index)
        {
            var t = itemInfos.RemoveAt(index);
            if (t != null)
                ReferencePool.Release(t);
        }

        public void SubItemInfoWithId(int id, int changeCount)
        {
            var list = itemInfos.Get(id);
            if (list == null)
                return;
            foreach (var itemInfo in list)
            {
                itemInfo.Count -= changeCount;
                if (itemInfo.Count <= 0)
                {
                    changeCount = Mathf.Abs(itemInfo.Count);
                    var t = itemInfos.RemoveAt(itemInfo.Index);
                    if (t != null)
                        ReferencePool.Release(t);
                    if (changeCount == 0)
                        break;
                }
                else
                {
                    break;
                }
            }
        }

        public int ReturnRemaining(int id, int count)
        {
            var configItem = Tables.Instance.ItemTable.GetOrDefault(id);
            var maxStack = configItem.MaxStackCount;
            var list = itemInfos.Get(id);
            if (list != null)
            {
                foreach (var item in list)
                {
                    int benci = Mathf.Min(maxStack - item.Count, count);
                    count -= benci;
                    if (count <= 0)
                        break;
                }
            }

            int canGetCount = itemInfos.GetNullCount();
            while (canGetCount != 0 && count > 0)
            {
                int benci = Mathf.Min(maxStack, count);
                count -= benci;
                canGetCount--;
            }

            return count;
        }

        public void AddItemInfoWithId(int id, int count)
        {
            if (count == 0)
                return;
            var configItem = Tables.Instance.ItemTable.GetOrDefault(id);
            var maxStack = configItem.MaxStackCount;
            var list = itemInfos.Get(id);
            if (list != null)
                foreach (var item in list)
                {
                    int benci = Mathf.Min(maxStack - item.Count, count);
                    item.Count += benci;
                    count -= benci;
                    if (count <= 0)
                        break;
                }

            while (count > 0)
            {
                int benci = Mathf.Min(maxStack, count);
                var itemInfo = ReferencePool.Acquire<ItemInfo>();
                itemInfo.Item = configItem;
                itemInfo.Count = benci;
                itemInfos.Add(id, itemInfo);
                count -= benci;
            }
        }

        public void AddItemInfoWithIndex(int index, int id, int count)
        {
            var itemInfo = ReferencePool.Acquire<ItemInfo>();
            var configItem = Tables.Instance.ItemTable.GetOrDefault(id);
            itemInfo.Item = configItem;
            itemInfo.Count = count;
            itemInfos.AddAt(index, id, itemInfo);
        }
    }
}