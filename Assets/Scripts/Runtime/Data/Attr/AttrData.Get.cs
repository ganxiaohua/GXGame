namespace GamePlay.Runtime
{
    public partial class AttrData
    {
        public int GetHp()
        {
            return attr[AttrData_Index.HP];
        }

        public int GetAtk()
        {
            return attr[AttrData_Index.Atk];
        }
    }
}