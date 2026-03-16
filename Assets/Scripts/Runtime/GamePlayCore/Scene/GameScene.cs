using Cysharp.Threading.Tasks;
using FairyGUI;
using GameFrame.Runtime;
using Gameplay.Runtime;
using UnityEngine;

namespace GamePlay.Runtime
{
    public class GameScene : SceneBase, IWorldEvent
    {
        protected override string SingleSceneName => "Game";

        protected override void OnReady()
        {
            UIConfig.defaultFont = "SIMHEI";
            DataManager.Instance.Initialization();
            ConstEasyUI.Init();
            Random.InitState((int) System.DateTime.Now.Ticks);
            Stage.inst.onStageResized.Add(() => { EventSend.Instance.FireUIEvent(UIEventMsg.IResizeWindow, null); });
            UISystem.Instance.ShowUniquePanelAsync<MainPanel>().Forget();
            AddComponent<MagicWorld, int>(AllComponents.TotalComponents);
#if UNITY_EDITOR
            DebugSceneView.Init();
#endif

            GameInput.Instance.InputTest();
            TimeData.Instance.SetTime();
        }

        public override void Dispose()
        {
#if UNITY_EDITOR
            DebugSceneView.Clear();
#endif
            base.Dispose();
            DataManager.Instance.ShutDown();
        }

        public void WorldEvent(WorldEventMsg msg, object obj)
        {
            switch (msg)
            {
                case WorldEventMsg.RefreshDate:
                    UISystem.Instance.ShowUniquePanelAsync<NextDayPanel>().Forget();
                    break;
            }
        }
    }
}