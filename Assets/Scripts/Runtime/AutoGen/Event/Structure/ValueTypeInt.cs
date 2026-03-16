using System;

namespace GamePlay.Runtime
{
    public class ValueTypeInt : IDisposable
    {
        public int Value1;
        public int Value2;

        public void Dispose()
        {
            Value1 = 0;
            Value2 = 0;
        }
    }
}