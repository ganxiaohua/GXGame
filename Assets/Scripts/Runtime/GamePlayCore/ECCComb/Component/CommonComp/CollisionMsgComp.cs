using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public struct CollisionMsg
    {
        public float groundDist;
        public float epsilon;
        public float skinWidth;
        public float anglePower;
        public float maxWalkingAngle;
        public float maxJumpAngle;
        public float jumpAngleWeightFactor;
        public float stepUpDepth;
        public int MaskLayer;

        public CollisionMsg(float groundDistt = 0.01f, float epsilont = 0.001f, float skinWidtht = 0.01f, float anglePowert = 2.0f,
                float maxWalkingAnglet = 60f, float maxJumpAnglet = 70f, float jumpAngleWeightFactort = 0.1f, float stepUpDeptht = 0.5f, int maskLayert = int.MaxValue)
        {
            this.groundDist = groundDistt;
            this.epsilon = epsilont;
            this.skinWidth = skinWidtht;
            this.anglePower = anglePowert;
            this.maxWalkingAngle = maxWalkingAnglet;
            this.maxJumpAngle = maxJumpAnglet;
            this.jumpAngleWeightFactor = jumpAngleWeightFactort;
            this.stepUpDepth = stepUpDeptht;
            this.MaskLayer = maskLayert;
        }
    }

    public struct CollisionMsgComp : EffComponent
    {
        public CollisionMsg Value;

        public void Dispose()
        {
        }
    }
}