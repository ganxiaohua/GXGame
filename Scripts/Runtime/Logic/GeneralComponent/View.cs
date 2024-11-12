using System;

namespace GameFrame
{
    public class View : ECSComponent
    {
        public IEceView Value;

        public static IEceView Create(Type type)
        {
            return (IEceView)ReferencePool.Acquire(type);
        }

        public override void Dispose()
        {
            ReferencePool.Release(Value);
        }
    }
    
}