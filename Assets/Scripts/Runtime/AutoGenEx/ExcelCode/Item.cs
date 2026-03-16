namespace GamePlay.Runtime
{
    public partial class Item
    {
        public bool IsStack => MaxStackCount > 1;
    }
}