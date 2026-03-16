using System.Threading;
using Cysharp.Threading.Tasks;
using FairyGUI;
using GameFrame.Runtime;
using GamePlay.Runtime;
using UnityEngine;
using UnityGXGameFrame.Runtime;

namespace Gameplay.Runtime
{
    public partial class MainBagPanel : TransitionPanel, IUIEvent
    {
        public override string Package => "Bag";
        public override string Name => "MainBag";

        public override PanelMode Mode => PanelMode.Mono;

        private LoopList<UIDrayItemLoop, UIItemInfo> pocketloopList;

        private LoopList<UIDrayItemLoop, UIItemInfo> bagLoopList;

        private Vector2 cellSizeWithPocket;

        private Vector2 cellSizeWithBag;

        private UIItemInfo uiItemInfoData;

        public override async UniTask OnInitializeAsync(GComponent root, CancellationToken cancelToken = default)
        {
            await base.OnInitializeAsync(root, cancelToken);
            InitializeComponents(root);
            n8.onClick.Add(() => { UISystem.Instance.HidePanel(this); });
            pocketloopList = new LoopList<UIDrayItemLoop, UIItemInfo>(n4_n6, this, OnPacketLoopList);
            bagLoopList = new LoopList<UIDrayItemLoop, UIItemInfo>(n4_n7, this, OnBagLoopList);
            CapRect();
            uiItemInfoData = new UIItemInfo();
        }

        public override void OnShow(object args = null)
        {
            base.OnShow(args);
            pocketloopList.Count = BagData.Instance.PocketItems.Count;
            bagLoopList.Count = BagData.Instance.BagItems.Count;
            GameInput.Instance.SetCommonState(false);
        }

        public override void OnHide()
        {
            GameInput.Instance.SetCommonState(true);
            base.OnHide();
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void CapRect()
        {
            var size = n4_n6.size * GRoot.inst.scale;
            cellSizeWithPocket = new Vector2(size.x / ConstData.PocketSizeX, size.y / ConstData.PocketSizeY);
            size = n4_n7.size * GRoot.inst.scale;
            cellSizeWithBag = new Vector2(size.x / ConstData.BagSizeX, size.y / ConstData.BagSizeY);
        }

        private UIItemInfo OnPacketLoopList(int index)
        {
            var item = BagData.Instance.GetPocketItemWithIndex(index);
            uiItemInfoData.itemInfo = item;
            uiItemInfoData.type = BagData.BagType.Pocket;
            return uiItemInfoData;
        }

        private UIItemInfo OnBagLoopList(int index)
        {
            var item = BagData.Instance.GetBagItemWithIndex(index);
            uiItemInfoData.itemInfo = item;
            uiItemInfoData.type = BagData.BagType.Bag;
            return uiItemInfoData;
        }

        private void PullGarbage(UIDragData uiDragData)
        {
            var rectn4N6 = n4_n6.Rect2Ins();
            var rectn4N7 = n4_n7.Rect2Ins();
            var drayCenter = uiDragData.Rect.center;
            bool efficient = false;
            if (rectn4N6.Contains(drayCenter))
            {
                var localPos = drayCenter - rectn4N6.position;
                int x = (int) (localPos.x / cellSizeWithPocket.x);
                int y = (int) (localPos.y / cellSizeWithPocket.y);
                BagData.Instance.ExchangeProps(uiDragData.Type, uiDragData.Index, BagData.BagType.Pocket, y * ConstData.PocketSizeY + x);
                efficient = true;
            }
            else if (rectn4N7.Contains(drayCenter))
            {
                var localPos = drayCenter - rectn4N7.position;
                int x = (int) (localPos.x / cellSizeWithBag.x);
                int y = (int) (localPos.y / cellSizeWithBag.y);
                BagData.Instance.ExchangeProps(uiDragData.Type, uiDragData.Index, BagData.BagType.Bag, y * ConstData.BagSizeX + x);
                efficient = true;
            }
            else
            {
                var rect = n10.Rect2Ins();
                //是否垃圾桶
                bool overlaps = uiDragData.Rect.Overlaps(rect);
                if (uiDragData.Rect.Overlaps(rect))
                {
                    BagData.Instance.RemoveItemWithIndex(uiDragData.Type, uiDragData.Index);
                }

                efficient = true;
            }

            uiDragData.Action?.Invoke(efficient);
            pocketloopList.Count = BagData.Instance.PocketItems.Count;
            bagLoopList.Count = BagData.Instance.BagItems.Count;
        }

        public void OnBagSelectChange(UIEventMsg msg, object obj)
        {
            switch (msg)
            {
                case UIEventMsg.IDragPocketItem:
                    PullGarbage((UIDragData) obj);
                    break;
                case UIEventMsg.IResizeWindow:
                    CapRect();
                    break;
            }
        }
    }
}