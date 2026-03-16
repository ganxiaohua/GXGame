namespace GamePlay.Runtime
{
    public partial class UnitItem
    {
        public Item GetItem()
        {
            return Tables.Instance.ItemTable.GetOrDefault(Id);
        }
    }
}