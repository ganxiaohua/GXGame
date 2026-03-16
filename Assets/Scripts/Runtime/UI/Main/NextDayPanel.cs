using System.Threading;
using Cysharp.Threading.Tasks;
using FairyGUI;
using GameFrame.Runtime;
using GamePlay.Runtime;


namespace Gameplay.Runtime
{
    public partial class NextDayPanel : TransitionPanel
    {
        public override string Package => "Main";
        public override string Name => "NextDay";

        public UniqueTimer UniqueTimer;

        public override async UniTask OnInitializeAsync(GComponent root, CancellationToken cancelToken = default)
        {
            await base.OnInitializeAsync(root, cancelToken);
            InitializeComponents(root);
            UniqueTimer = new UniqueTimer(OnQuit);
        }

        public override void OnShow(object args = null)
        {
            base.OnShow(args);
            UniqueTimer.Schedule(2);
            GameInput.Instance.SetCommonState(false);
        }

        public override void OnHide()
        {
            base.OnHide();
            GameInput.Instance.SetCommonState(true);
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        private void OnQuit()
        {
            UISystem.Instance.HidePanel(this);
        }
    }
}