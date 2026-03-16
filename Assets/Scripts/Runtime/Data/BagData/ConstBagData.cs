namespace GamePlay.Runtime
{
    public static class ConstBagData
    {
        public static bool BagResIsEnough(ItemCount needitem)
        {
            var bagItem = BagData.Instance.GetItemCount(needitem.Item);
            return bagItem - needitem.Count >= 0;
        }

        public static (int need, int bag) ResNeedandBagCount(ItemCount needitem)
        {
            int bagCount = 0;
            bagCount = BagData.Instance.GetItemCount(needitem.Item);
            return (needitem.Count, bagCount);
        }
    }
}