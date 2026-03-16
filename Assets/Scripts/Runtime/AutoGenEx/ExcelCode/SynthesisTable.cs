using System.Collections.Generic;

namespace GamePlay.Runtime
{
    public partial class SynthesisTable
    {
        private Dictionary<ItemType, List<Item>> classifyTable;
        private List<ItemType> typeList;
        private void Classify()
        {
            if (classifyTable != null)
                return;
            classifyTable = new();
            typeList = new();
            foreach (var data in DataList)
            {
                if (!classifyTable.TryGetValue(data.SyItem_Ref.ItemType, out var hashSet))
                {
                    hashSet = new List<Item>();
                    classifyTable.Add(data.SyItem_Ref.ItemType,hashSet);
                    typeList.Add(data.SyItem_Ref.ItemType);
                }
                hashSet.Add(data.SyItem_Ref);
            }
        }

        public Dictionary<ItemType,List<Item>> GetClassifyTable()
        {
            Classify();
            return classifyTable;
        }

        public  List<ItemType> GetItemType()
        {
            Classify();
            return typeList;
        }
    }
}