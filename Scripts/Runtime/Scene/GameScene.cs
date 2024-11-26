using Cysharp.Threading.Tasks;
using FairyGUI;
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
            AddComponent<GameWrold>();
        }

        private void CameraSet()
        {
            var uiCamera =  GameObject.Find("Stage Camera").GetComponent<Camera>();
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            var cameraData = Camera.main.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(uiCamera);
            UIManager.Instance.OpenUI(typeof(UICardListWindow), "input Data__");
        }
    }
}