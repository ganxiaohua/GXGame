using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    // [ViewBind(typeof(ICameraLensZoomAngle))]
    public struct CameraLensZoomAngleComp : EffComponent
    {
        public float Value;

        public void Dispose()
        {
        }
    }
}