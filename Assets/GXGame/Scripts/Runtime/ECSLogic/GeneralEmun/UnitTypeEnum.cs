namespace GXGame
{
    /// <summary>
    /// 单位类型
    /// </summary>
    public enum UnitTypeEnum
    {
        BASIC = 1,          //基本
        BUILDING = 1<<1,   //建筑物
        CUSTOM = 1<<2,     //普通
        HERO = 1<<3,       //英雄
        MECHANICAL = 1<<4, //机械
        MONSER = 1<<5,     //怪物
        TREE = 1<<6,       //树木
    }
}