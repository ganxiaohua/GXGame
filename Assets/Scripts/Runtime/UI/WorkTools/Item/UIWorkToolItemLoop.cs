using FairyGUI;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class UIWorkToolItemLoop: LoopItem<Item>
    {
        private UIItem uiItem;
        private FairyGUI.GTextField title;
        private FairyGUI.Controller c1;
        public override void OnAwake(GComponent com, ILoopList owner)
        {
            base.OnAwake(com,owner);
            title = (GTextField) com.GetChild("name");
            uiItem = ReferencePool.Acquire<UIItem>();
            uiItem.Init((GComponent)com.GetChild("IconItem"),OwnerPanel.AssetReference);
            c1 = com.GetController("c1");
            com.onClick.Add(OnSelect);
        }

        public override void OnShow(int globalIndex, Item data, bool selected)
        {
            base.OnShow(globalIndex, data, selected);
            title.text = data.Name;
            uiItem.Show(data);
        }
        
        private void OnSelect()
        {
            Select();
        }
        
        protected override void OnSelectChanged()
        {
            c1.selectedIndex = Selected ? 1 : 0;
        }
        
        public override void OnDestroy()
        {
            ReferencePool.Release(uiItem);
        }
    }
}