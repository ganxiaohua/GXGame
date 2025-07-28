using System;

namespace GameFrame.Runtime
{
    public class View : EffComponent
    {
        public IEceView Value;

        public static IEceView Create(Type type)
        {
            return (IEceView) ReferencePool.Acquire(type);
        }

        public override void Dispose()
        {
            ReferencePool.Release(Value);
        }
    }
}