using Cinemachine;
using GameFrame.Runtime;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace GamePlay.Runtime
{
    public class CameraView : EffEntityView, ICameraLensZoomAngle
    {
        public CinemachineBrain cinemachineBrain;
        public CinemachineVirtualCamera cinemachineVirtualCamera1;

        public CinemachineVirtualCamera cinemachineVirtualCamera2;

        public Camera camera;

        // private CinemachineVirtualCamera cinemachineVirtualCamera;
        private CinemachineTransposer transposerBody;


        protected override void OnAfterBind(GameObject go)
        {
            base.OnAfterBind(go);
            cinemachineBrain = BindingTarget.gameObject.GetComponentInChildren<CinemachineBrain>();
            camera = BindingTarget.gameObject.GetComponentInChildren<Camera>();
            cinemachineVirtualCamera1 = go.transform.Find("Virtual_1").GetComponentInChildren<CinemachineVirtualCamera>();
            cinemachineVirtualCamera2 = go.transform.Find("Virtual_2").GetComponentInChildren<CinemachineVirtualCamera>();
            transposerBody = (CinemachineTransposer) cinemachineVirtualCamera1.GetCinemachineComponent(CinemachineCore.Stage.Body);
            SetFollow();
            SetOverCamera();
            cinemachineVirtualCamera1.Priority = 11;
            cinemachineVirtualCamera2.Priority = 10;
        }

        private void SetOverCamera()
        {
            var uiCamera = FairyGUI.StageCamera.main;
            uiCamera.GetUniversalAdditionalCameraData().renderType = CameraRenderType.Overlay;
            var cameraData = camera.GetUniversalAdditionalCameraData();
            cameraData.cameraStack.Add(uiCamera);
        }

        private void SetFollow()
        {
            var id = BindEntity.GetCameraWatcherComp().Value;
            var target = BindEntity.world.GetChild(id);
            if (target == null)
                return;
            if (!target.HasComponent<View>())
                return;
            var targetGo = (ManView) (target.GetView().GetData());
            cinemachineVirtualCamera1.Follow = targetGo.ViewPoint.transform;
            cinemachineVirtualCamera1.LookAt = targetGo.ViewPoint.transform;
            cinemachineVirtualCamera2.Follow = targetGo.ViewPoint.transform;
        }

        public override void TickActive(float delatTime, float realElapseSeconds)
        {
            if (transposerBody == null)
                return;
            var cameraLensZoomAngleComp = BindEntity.GetCameraLensZoomAngleComp();
            float rad = cameraLensZoomAngleComp.Value * Mathf.Deg2Rad;
            float y = Mathf.Cos(rad) * ConstData.Camera2FollowerTrackRadius + ConstData.Camera2FollowerTrackRadius;
            float z = -Mathf.Sin(rad) * ConstData.Camera2FollowerTrackRadius;
            Vector3 src = transposerBody.m_FollowOffset;
            Vector3 dst = new Vector3(0, y, z);
            transposerBody.m_FollowOffset = Vector3.Lerp(src, dst, delatTime * 5);
        }

        public void ICameraLensZoomAng(CameraLensZoomAngleComp comp)
        {
            // Debug.Log("xx");
        }
    }
}