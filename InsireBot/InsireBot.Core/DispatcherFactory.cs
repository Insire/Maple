using System;
using System.Windows.Threading;

namespace Maple.Core
{
    /// <summary>
    /// 
    /// </summary>
    public static class DispatcherFactory
    {
        /// <summary>
        /// Gets the dispatcher.
        /// </summary>
        /// <returns></returns>
        public static Dispatcher GetDispatcher()
        {
            return Dispatcher.CurrentDispatcher;
        }

        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        /// <param name="action">The action.</param>
        public static void Invoke(Action action)
        {
            var dispatcher = GetDispatcher();

            if (dispatcher.CheckAccess())
                action();
            else
                dispatcher.BeginInvoke(action);
        }

        /// <summary>
        /// Invokes the specified action.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="action">The action.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="priority">The priority.</param>
        public static void Invoke<T>(Action<T> action, T parameter, DispatcherPriority priority = DispatcherPriority.Normal)
        {
            var dispatcher = GetDispatcher();

            if (dispatcher.CheckAccess())
                action?.Invoke(parameter);
            else
                dispatcher.BeginInvoke(action, priority, parameter);
        }
    }
}
