using Eden.Gameplay.Runtime;
using GameFrame;
using GXGame.Logic;
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
            AddComponent<GameWorld>();
            UIManager.Instance.OpenUI(typeof(UICardListWindow), "input (自定义数据)");
            EventSend.Instance.FireTestEvent1("发送一个事件");
        }

        private void CameraSet()
        {
            var uiCamera =  GameObject.Find("Stage Camera").GetComponent<Camera>();
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(uiCamera);
        }
    }
}