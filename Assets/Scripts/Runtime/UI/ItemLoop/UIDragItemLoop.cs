using FairyGUI;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class UIItemInfo
    {
        public ItemInfo itemInfo;
        public BagData.BagType type;
    }

    public class UIDrayItemLoop : LoopItem<UIItemInfo>
    {
        private DragUIItem uiItem;

        public override void OnAwake(GComponent com, ILoopList owner)
        {
            base.OnAwake(com, owner);
            uiItem = ReferencePool.Acquire<DragUIItem>();
            uiItem.Init(com, OwnerPanel.AssetReference);
        }

        public override void OnShow(int globalIndex, UIItemInfo data, bool selected)
        {
            base.OnShow(globalIndex, data, selected);
            uiItem.Show(data.itemInfo);
            uiItem.Type = data.type;
            uiItem.SetDataIndex(globalIndex);
        }

        protected override void OnSelectChanged()
        {
            uiItem.Selet(Selected);
        }

        public override void OnDestroy()
        {
            ReferencePool.Release(uiItem);
        }
    }
}