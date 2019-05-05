using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

using Maple.Core;

namespace Maple
{
    public abstract class DialogBaseViewModel : ViewModel
    {
        public EventHandler DialogClosed;

        public ExceptionContentDialogViewModel ExceptionDialogViewModel { get; protected set; }
        public MessageContentDialogViewModel MessageDialogViewModel { get; protected set; }
        public ProgressContentDialogViewModel ProgressDialogViewModel { get; protected set; }

        public ICommand CloseDialogCommand { get; protected set; }
        public ICommand CancelDialogCommand { get; protected set; }
        public ICommand AcceptDialogCommand { get; protected set; }

        public Action AcceptAction { get; protected set; }
        public Action CancelAction { get; protected set; }

        public Func<bool> CanCancelFunc { get; protected set; }
        public Func<bool> CanAcceptFunc { get; protected set; }

        private bool _isOpen;
        public bool IsOpen
        {
            get { return _isOpen; }
            set { SetValue(ref _isOpen, value, OnChanged: OnOpenChanged); }
        }

        private bool _isCancelVisible;
        public bool IsCancelVisible
        {
            get { return _isCancelVisible; }
            set { SetValue(ref _isCancelVisible, value); }
        }

        private string _title;
        public string Title
        {
            get { return _title; }
            protected set { SetValue(ref _title, value); }
        }

        private string _titleDetail;
        public string TitleDetail
        {
            get { return _titleDetail; }
            protected set { SetValue(ref _titleDetail, value); }
        }

        private ObservableObject _context;
        public ObservableObject Context
        {
            get { return _context; }
            set { SetValue(ref _context, value); }
        }

        protected DialogBaseViewModel(IMessenger messenger)
            : base(messenger)
        {
        }

        private void OnOpenChanged()
        {
            if (!IsOpen)
                DialogClosed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Accepts this instance.
        /// </summary>
        public void Accept()
        {
            AcceptAction?.Invoke();
            Close();
        }

        /// <summary>
        /// Determines whether this instance can accept.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can accept; otherwise, <c>false</c>.
        /// </returns>
        public bool CanAccept()
        {
            return CanClose() && (CanAcceptFunc?.Invoke() ?? true) == true;
        }

        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            CancelAction?.Invoke();
            Close();
        }

        /// <summary>
        /// Determines whether this instance can cancel.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can cancel; otherwise, <c>false</c>.
        /// </returns>
        public bool CanCancel()
        {
            return CanClose() && (CanCancelFunc?.Invoke() ?? true) == true;
        }

        public Task Open()
        {
            return Open(CancellationToken.None);
        }

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
        public async Task Open(CancellationToken token)
        {
            var tcs = new TaskCompletionSource<object>();
            var registration = token.Register(() => tcs.TrySetCanceled());
            void lambda(object s, EventArgs e) => tcs.TrySetResult(null);
            try
            {
                DialogClosed += lambda;
                IsOpen = true; // open dialog
                await tcs.Task.ConfigureAwait(false); // wait for dialog to close
            }
            finally
            {
                DialogClosed -= lambda;
                registration.Dispose();
            }
        }

        /// <summary>
        /// Closes this instance.
        /// </summary>
        public void Close()
        {
            IsOpen = false;
        }

        /// <summary>
        /// Determines whether this instance can close.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can close; otherwise, <c>false</c>.
        /// </returns>
        public bool CanClose()
        {
            return IsOpen;
        }
    }
}
