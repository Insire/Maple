using System;
using System.Threading.Tasks;

namespace Maple.Core
{
    public sealed class NotifyTaskCompletion<TResult> : ObservableObject
    {
        private readonly IMapleLog _log;
        private readonly Task<TResult> _task;

        public AggregateException Exception { get { return _task.Exception; } }
        public TaskStatus Status { get { return _task.Status; } }

        public bool IsCompleted { get { return _task.IsCompleted; } }
        public bool IsNotCompleted { get { return !_task.IsCompleted; } }
        public bool IsCanceled { get { return _task.IsCanceled; } }
        public bool IsFaulted { get { return _task.IsFaulted; } }

        public NotifyTaskCompletion(Task<TResult> task, IMapleLog log)
        {
            _task = task ?? throw new ArgumentNullException(nameof(task));
            _log = log ?? throw new ArgumentNullException(nameof(log));

            if (!task.IsCompleted)
            {
                var _ = WatchTaskAsync(task);
            }
        }

        private async Task WatchTaskAsync(Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                _log.Error(ex);
            }

            OnPropertyChanged(nameof(Status));
            OnPropertyChanged(nameof(IsCompleted));
            OnPropertyChanged(nameof(IsNotCompleted));

            if (task.IsCanceled)
                OnPropertyChanged(nameof(IsCanceled));
            else if (task.IsFaulted)
            {
                OnPropertyChanged(nameof(IsFaulted));
                OnPropertyChanged(nameof(Exception));
                OnPropertyChanged(nameof(InnerException));
                OnPropertyChanged(nameof(ErrorMessage));
            }
            else
            {
                OnPropertyChanged(nameof(IsSuccessfullyCompleted));
                OnPropertyChanged(nameof(Result));
            }
        }

        public TResult Result
        {
            get { return (_task.Status == TaskStatus.RanToCompletion) ? _task.Result : default(TResult); }
        }

        public bool IsSuccessfullyCompleted
        {
            get { return _task.Status == TaskStatus.RanToCompletion; }
        }

        public Exception InnerException
        {
            get { return Exception?.InnerException; }
        }

        public string ErrorMessage
        {
            get { return InnerException?.Message; }
        }
    }
}
