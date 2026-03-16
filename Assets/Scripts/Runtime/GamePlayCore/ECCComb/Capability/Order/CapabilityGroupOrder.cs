namespace GamePlay.Runtime
{
    public class CapabilityGroupOrder
    {
        private const int Order1 = 0;
        private const int Order2 = 100;
        private const int Order3 = 200;
        private const int Order4 = 300;
        private const int Order5 = 400;
        private const int Order6 = 500;
        private const int Order7 = 600;

        public const int Enter = Order1;
        public const int BeBavior = Order1 + 2;
        public const int KCC = Order1 + 3;
        public const int CommonMove = Order1 + 4;
        public const int Fly = Order1 + 5;
        public const int DetectionLogo = Order1 + 6;
        public const int OperatedCapabiity = Order2;
        public const int OperatedCountdown = Order2 + 1;
        public const int Throw = Order2 + 2;
        public const int OperatedCountdownMonster = Order2 + 2;
        public const int AlwaysOperatedDetection = Order2 + 3;
        public const int UpdateCollision = Order2 + 4;
        public const int OperatedDetectionCapability = Order2 + 5;
        public const int PlayerAccumulate = Order2 + 6;
        public const int OperatedExecute = Order2 + 7;
        public const int OperatedOver = Order2 + 8;
        public const int BeAttackAttr = Order2 + 9;
        public const int BeAttackBehavior = Order2 + 10;
        public const int BeAttackPower = Order2 + 11;

        public const int BeUser = Order3;
        public const int ShowItemQua = Order3;
        public const int JumpoutHarvest = Order3 + 1;
        public const int HarvestAbsorb = Order3 + 2;
        public const int View = Order4 + 1;
        public const int Die = Order4 + 2;
        public const int CameraEnter = Order5;
        public const int ClearEnter = Order7;
    }
}