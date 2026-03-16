namespace GamePlay.Runtime
{
    public static class ConstUIText
    {
        public static string NeedItemBagItem(ItemCount needItem)
        {
            if (needItem == null)
                return "";
            var counts = ConstBagData.ResNeedandBagCount(needItem);
            string colorStr = ConstBagData.BagResIsEnough(needItem) ? ConstColor.ResAdeStr : ConstColor.NotResAdeStr;
            return $"[color={colorStr}]{counts.need}/{counts.bag}[/color]";
        }
    }
}