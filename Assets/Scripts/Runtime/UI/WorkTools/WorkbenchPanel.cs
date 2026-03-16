using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using FairyGUI;
using GameFrame.Runtime;
using GamePlay.Runtime;

namespace Gameplay.Runtime
{
    public partial class WorkbenchPanel : TransitionPanel
    {
        public override string Package => "WorkTools";
        public override string Name => "Workbench";

        public override PanelMode Mode => PanelMode.Mono;

        private LoopList<UITextItemLoop, string> typeLoop;


        private LoopList<UIWorkToolItemLoop, Item> catalogueLoop;

        private List<Item> itemList;

        private List<ItemType> itemTypes = Tables.Instance.SynthesisTable.GetItemType();

        private UIItem[] uiItem;

        private int typeIndex;

        private int secondaryMenuIndex;

        public override async UniTask OnInitializeAsync(GComponent root, CancellationToken cancelToken = default)
        {
            await base.OnInitializeAsync(root, cancelToken);
            InitializeComponents(root);
            n17.onClick.Add(() => { UISystem.Instance.HidePanel(this); });
            typeLoop = new LoopList<UITextItemLoop, string>(n7_n10, this, OnTypeLoop);
            typeLoop.OnSelectItem = OnTypeLoopSelect;
            catalogueLoop = new LoopList<UIWorkToolItemLoop, Item>(n7_n13, this, WorkToolItemLoop);
            catalogueLoop.OnSelectItem = OnWorkToolItemSelect;
            //TODO:会发生变化数量
            uiItem = new UIItem[3];
            var item = ReferencePool.Acquire<UIItem>();
            item.Init(n12_n5, AssetReference);
            uiItem[0] = item;
            item = ReferencePool.Acquire<UIItem>();
            item.Init(n12_n6, AssetReference);
            uiItem[1] = item;
            item = ReferencePool.Acquire<UIItem>();
            item.Init(n12_n9, AssetReference);
            uiItem[2] = item;
            n12_n11.onClick.Add(OnSynthesis);
        }

        public override void OnShow(object args = null)
        {
            base.OnShow(args);
            typeIndex = 0;
            typeLoop.Count = itemTypes.Count;
            typeLoop.Select(0);
        }

        public override void OnHide()
        {
            base.OnHide();
            catalogueLoop.SelectClear();
            typeLoop.SelectClear();
        }

        public override void Dispose()
        {
            typeLoop.Dispose();
            catalogueLoop.Dispose();
            foreach (var item in uiItem)
            {
                ReferencePool.Release(item);
            }

            GObjectPools.Instance.Unspawn<HintObject>();
            base.Dispose();
        }

        private void Refresh1()
        {
            catalogueLoop.SelectClear();
            itemList = Tables.Instance.SynthesisTable.GetClassifyTable()[itemTypes[typeIndex]];
            catalogueLoop.Count = itemList.Count;
            secondaryMenuIndex = 0;
            catalogueLoop.Select(0);
            Refresh2();
        }

        private void Refresh2()
        {
            var formula = Tables.Instance.SynthesisTable.GetOrDefault(itemList[secondaryMenuIndex].Id);
            var items = formula.Items;
            for (int i = 0; i < items.Length; i++)
            {
                uiItem[i].Show(items[i]);
                uiItem[i].SetCount(ConstUIText.NeedItemBagItem(items[i]));
            }

            uiItem[^1].Show(formula.SyItem_Ref);
        }

        private string OnTypeLoop(int index)
        {
            return itemTypes[index].ToString();
        }

        private void OnTypeLoopSelect(ILoopItem item)
        {
            typeIndex = item.GlobalIndex;
            Refresh1();
        }


        private Item WorkToolItemLoop(int index)
        {
            return itemList[index];
        }

        private void OnWorkToolItemSelect(ILoopItem item)
        {
            secondaryMenuIndex = item.GlobalIndex;
            Refresh2();
        }

        private void OnSynthesis()
        {
            bool enough = true;
            var formula = Tables.Instance.SynthesisTable.GetOrDefault(itemList[secondaryMenuIndex].Id);
            var items = formula.Items;
            for (int i = 0; i < items.Length; i++)
            {
                if (ConstBagData.BagResIsEnough(items[i])) continue;
                enough = false;
                break;
            }

            if (!enough)
            {
                Debugger.Log("东西不足!");
            }
            else
            {
                var succ = BagData.Instance.AddItem(formula.SyItem.Value, 1);
                if (succ)
                {
                    for (int i = 0; i < items.Length; i++)
                    {
                        BagData.Instance.SubItem(items[i].Item, items[i].Count);
                    }

                    Refresh2();
                }
                else
                {
                    Debugger.Log("背包空间不足!");
                }
            }
        }
    }
}