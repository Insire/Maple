using System;
using System.Diagnostics;

namespace Maple.Core
{
    /// <summary>
    /// Represents the result of executing a process.
    /// </summary>
    public sealed class ProcessExecutionResult : IDisposable
    {
        private Process _process;
        private bool _disposed;

        internal ProcessExecutionResult(Process process, string[] standardOutput, string[] standardError)
        {
            _process = Ensure.NotNull(process, nameof(process));

            PID = _process.Id;
            ExecutionTime = _process.ExitTime - _process.StartTime;
            StandardOutput = Ensure.NotNull(standardOutput, nameof(standardOutput));
            StandardError = Ensure.NotNull(standardError, nameof(standardError));
        }

        /// <summary>
        /// Gets the process ID.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        public int PID { get; }

        /// <summary>
        /// Gets the execution time of the process.
        /// </summary>
        public TimeSpan ExecutionTime { get; }

        /// <summary>
        /// Gets the standard output of the process.
        /// </summary>
        public string[] StandardOutput { get; }

        /// <summary>
        /// Gets the standard error of the process.
        /// </summary>
        public string[] StandardError { get; }

        /// <summary>
        /// Read the value of the process property identified by the given <paramref name="selector"/>.
        /// </summary>
        public T ReadProcessInfo<T>(Func<Process, T> selector)
        {
            return selector(_process);
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases all resources used by the underlying process.
        /// </summary>
        private void Dispose(bool disposing)
        {
            if (_disposed)
                return;

            if (disposing)
            {
                if (_process != null)
                {
                    _process.Dispose();
                    _process = null;
                }
            }

            _disposed = true;
        }
    }
}
