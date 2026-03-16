using FairyGUI;
using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public class UIItemLoop : LoopItem<ItemInfo>
    {
        private UIItem uiItem;

        public override void OnAwake(GComponent com, ILoopList owner)
        {
            base.OnAwake(com, owner);
            uiItem = ReferencePool.Acquire<UIItem>();
            uiItem.Init(com, OwnerPanel.AssetReference);
        }

        public override void OnShow(int globalIndex, ItemInfo data, bool selected)
        {
            base.OnShow(globalIndex, data, selected);
            uiItem.Show(data);
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