using System;
using GameFrame.Runtime;
using Sirenix.OdinInspector;

namespace GamePlay.Runtime
{
    [Serializable]
    public partial class AttrData : IDisposable
    {
        [ShowInInspector]
        private int[] attr;

        public void GetAttrWithConfig(AttributeItem item)
        {
            attr = new int[8];
            attr[AttrData_Index.Atk] = item.Atk;
            attr[AttrData_Index.HP] = item.HP;
        }

        public void AddAttr(AttrData changeData)
        {
            for (int i = 0; i < changeData.attr.Length; i++)
            {
                attr[i] += changeData.attr[i];
            }
        }

        public void SubAttr(AttrData changeData)
        {
            for (int i = 0; i < changeData.attr.Length; i++)
            {
                attr[i] -= changeData.attr[i];
            }
        }

        public void Dispose()
        {
            Array.Clear(attr, 0, attr.Length);
        }
    }
}