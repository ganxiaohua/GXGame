using System;

namespace GameFrame.Runtime
{
    public interface IEceView : IDisposable
    {
        public void Link(EffEntity entity);

        public void OnUpdate(float elapseSeconds, float realElapseSeconds);
    }

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