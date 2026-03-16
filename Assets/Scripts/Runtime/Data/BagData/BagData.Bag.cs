namespace GamePlay.Runtime
{
    public partial class BagData
    {
        public ItemInfoBulk BagItems { get; private set; }

        public int BagReturnRemaining(int id, int count)
        {
            return BagItems.ReturnRemaining(id, count);
        }


        public int GetBagItemCountWithId(int id)
        {
            return BagItems.GetCountWithId(id);
        }

        public ItemInfo GetBagItemWithIndex(int index)
        {
            return BagItems.GetItemInfoWithIndex(index);
        }

        private void RemoveBagItemWithIndex(int index)
        {
            BagItems.RmoveItemInfoWithIndex(index);
        }

        private void SubBagItemWithId(int id, int changeCount)
        {
            BagItems.SubItemInfoWithId(id, changeCount);
        }

        private void AddBagItemWithId(int id, int count)
        {
            BagItems.AddItemInfoWithId(id, count);
        }
    }
}