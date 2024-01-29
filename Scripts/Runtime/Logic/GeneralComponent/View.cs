namespace GameFrame
{
    public class View : ECSComponent
    {
        public IEceView Value;
        public override void Clear()
        {
            ReferencePool.Release(Value);
        }
    }
    
}