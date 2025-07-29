using Eden.Gameplay.Runtime;
using GameFrame.Runtime;
using GXGame.Runtime;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GXGame
{
    public class GameScene : SceneBase
    {
        protected override string SingleSceneName => "Scene_Game";

        protected override void OnReady()
        {
            CameraSet();
            AddComponent<ECSGameWorld, int>(AllComponents.TotalComponents);
            // AddComponent<ECCGameWorld, int>(AllComponents.TotalComponents);
            QualitySettings.vSyncCount = 0;
            UIManager.Instance.OpenUI(typeof(UICardListWindow), "input (自定义数据)");
            EventSend.Instance.FireTestEvent1("发送一个事件");
        }

        private void CameraSet()
        {
            var uiCamera = GameObject.Find("Stage Camera").GetComponent<Camera>();
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(uiCamera);
        }


        public override void Dispose()
        {
            base.Dispose();
        }
    }
}