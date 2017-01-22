using System;

namespace InsireBot.Core
{
    [Serializable]
    public class BusyToken : WeakReference, IDisposable
    {
        public bool Disposing { get; private set; }

        public BusyToken(BusyStack stack) : base(stack)
        {
            stack.Push(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!Disposing)
            {
                Disposing = true;

                var stack = Target as BusyStack;
                stack?.Pull();
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
