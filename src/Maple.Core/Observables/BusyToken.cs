using System;

namespace Maple.Core
{
    [Serializable]
    public sealed class BusyToken : WeakReference, IDisposable
    {
        public BusyToken(BusyStack stack)
            : base(stack)
        {
            stack.Push(this);
        }

        public void Dispose()
        {
            if (Target is BusyStack stack)
            {
                stack?.Pull();
            }
        }
    }
}
