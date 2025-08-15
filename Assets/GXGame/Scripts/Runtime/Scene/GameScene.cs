using GameFrame.Runtime;
using Gameplay.Runtime;
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
            SetCamera();
            //这里是走ecs路线
            // AddComponent<ECSGameWorld, int>(AllComponents.TotalComponents);
            //这里是走ecc路线
            AddComponent<ECCGameWorld, int>(AllComponents.TotalComponents);
            QualitySettings.vSyncCount = 0;
            EventSend.Instance.FireTestEvent1("发送一个事件");
        }

        private void SetCamera()
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