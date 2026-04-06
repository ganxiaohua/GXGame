using System.Threading;
using Cysharp.Threading.Tasks;
using FairyGUI;
using GameFrame.Runtime;
using GamePlay.Runtime;

namespace Gameplay.Runtime
{
    public partial class MainPanel : TransitionPanel, IUIEvent
    {
        public override string Package => "Main";
        public override string Name => "Main";
        private LoopList<UIItemLoop, ItemInfo> loopList;
        public override PanelMode Mode => PanelMode.Mono;

        private float interval;

        public override async UniTask OnInitializeAsync(GComponent root, CancellationToken cancelToken = default)
        {
            await base.OnInitializeAsync(root, cancelToken);
            InitializeComponents(root);
            loopList = new LoopList<UIItemLoop, ItemInfo>(n0, this, OnLoopList);
            n3_n9.min = 0;
            n3_n9.max = 1;
            n3_n8.min = 0;
            n3_n8.max = 1;
        }

        public override void OnShow(object args = null)
        {
            base.OnShow(args);
            loopList.Count = BagData.Instance.PocketItems.Count;
            loopList.Select(BagData.Instance.CurBagIndex);
            OnRefreshActionValue();
        }

        public override void OnRestore()
        {
            base.OnRestore();
            loopList.Count = BagData.Instance.PocketItems.Count;
            loopList.Select(BagData.Instance.CurBagIndex);
        }


        public override void OnHide()
        {
            base.OnHide();
        }

        public override void Dispose()
        {
            base.Dispose();
            DestroySingeItem();
            loopList.Dispose();
        }

        protected override void Update(float elapseSeconds, float realElapseSeconds)
        {
            OnShowTime(realElapseSeconds);
            var value = PlayerAttrData.Instance.activeValue;
            n3_n9.value = value.CurActiveValue / (value.MaxActiveValue * 1.0f);
        }

        private ItemInfo OnLoopList(int index)
        {
            return BagData.Instance.GetPocketItemWithIndex(index);
        }

        private void OnBagSelectChange()
        {
            loopList.Select(BagData.Instance.CurBagIndex);
        }

        private void OnRefreshBag()
        {
            loopList.Count = BagData.Instance.PocketItems.Count;
        }

        private void OnRefreshHp(ValueTypeInt value)
        {
            n3_n8.value = value.Value1 / (value.Value2 * 1.0f);
            // n3_n8_Cuttitle.text = $"{value.Value1}/{value.Value1}";
        }

        private void OnRefreshActionValue()
        {
            var value = PlayerAttrData.Instance.activeValue;
            n3_n9.value = value.CurActiveValue / (value.MaxActiveValue * 1.0f);
            // n3_n9_Cuttitle.text = string.Empty; //$"{value.CurActiveValue}/{value.MaxActiveValue}";
        }

        public void OnUIEvent(UIEventMsg msg, object obj)
        {
            switch (msg)
            {
                case UIEventMsg.IBagSelectEvent:
                    OnBagSelectChange();
                    break;
                case UIEventMsg.IRefreshBag:
                    OnRefreshBag();
                    break;
                case UIEventMsg.IRefreshHp:
                    OnRefreshHp((ValueTypeInt) obj);
                    break;
                case UIEventMsg.IRefreshActionValue:
                    OnRefreshActionValue();
                    break;
                case UIEventMsg.IShowIntroduce:
                    ShowIntroduceItem((UnitItem) obj, Root);
                    break;
                case UIEventMsg.IHideIntroduce:
                    HideIntroduceItem();
                    break;
            }
        }

        private void OnShowTime(float realtimeSinceStartup)
        {
            if (!(interval <= realtimeSinceStartup)) return;
            Time.text = TimeData.Instance.GetTimeString();
            interval = realtimeSinceStartup + 1;
        }
    }
}