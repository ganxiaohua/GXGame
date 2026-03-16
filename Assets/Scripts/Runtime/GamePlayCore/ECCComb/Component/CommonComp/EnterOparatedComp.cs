using GameFrame.Runtime;

namespace GamePlay.Runtime
{
    public struct OparatedData
    {
        public bool Oparated;
        public bool IsLongTouch;
    }

    public struct EnterOparatedComp : EffComponent
    {
        public OparatedData Value;

        public void Dispose()
        {
        }
    }

    public struct ThrowComp : EffComponent
    {
        public bool Value;

        public void Dispose()
        {
        }
    }
}