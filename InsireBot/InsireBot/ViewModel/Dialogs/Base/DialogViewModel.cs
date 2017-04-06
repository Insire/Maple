using Maple.Core;
using Maple.Localization.Properties;
using Maple.Youtube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple
{
    public class DialogViewModel : ObservableObject
    {
        private readonly IYoutubeUrlParseService _service;
        private readonly IMediaItemMapper _mediaItemMapper;

        public EventHandler DialogClosed;

        public ExceptionDialogViewModel ExceptionDialogViewModel { get; private set; }
        public FileBrowserDialogViewModel FileBrowserDialogViewModel { get; private set; }
        public FolderBrowserDialogViewModel FolderBrowserDialogViewModel { get; private set; }
        public MessageDialogViewModel MessageDialogViewModel { get; private set; }
        public ProgressDialogViewModel ProgressDialogViewModel { get; private set; }

        public ICommand CloseDialogCommand { get; private set; }
        public ICommand CancelDialogCommand { get; private set; }
        public ICommand AcceptDialogCommand { get; private set; }

        public Action AcceptAction { get; set; }
        public Action CancelAction { get; set; }
        public Func<bool> CanCancelFunc { get; set; }
        public Func<bool> CanAcceptFunc { get; set; }

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
            private set { SetValue(ref _title, value); }
        }

        private ObservableObject _context;
        public ObservableObject Context
        {
            get { return _context; }
            set { SetValue(ref _context, value); }
        }

        public DialogViewModel(IYoutubeUrlParseService service, IMediaItemMapper mediaItemMapper)
        {
            _service = service;
            _mediaItemMapper = mediaItemMapper;

            CloseDialogCommand = new RelayCommand(Close, () => CanClose());
            CancelDialogCommand = new RelayCommand(Cancel, () => CanCancel());
            AcceptDialogCommand = new RelayCommand(Accept, () => CanAccept());

            ExceptionDialogViewModel = new ExceptionDialogViewModel();
            FileBrowserDialogViewModel = new FileBrowserDialogViewModel();
            FolderBrowserDialogViewModel = new FolderBrowserDialogViewModel();
            MessageDialogViewModel = new MessageDialogViewModel();
            ProgressDialogViewModel = new ProgressDialogViewModel();
        }

        private void OnOpenChanged()
        {
            if (!IsOpen)
                DialogClosed?.Invoke(this, EventArgs.Empty);
        }

        public Task ShowMessageDialog(string message, string title)
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            Context = MessageDialogViewModel;
            Title = title;
            MessageDialogViewModel.Message = message;
            IsCancelVisible = false;

            return Open();
        }

        public Task ShowExceptionDialog(Exception exception)
        {
            if (IsOpen) // no exception spam, could probably be improved TODO ?
                return Task.FromResult(0);

            Context = ExceptionDialogViewModel;
            Title = exception.GetType().Name;
            ExceptionDialogViewModel.Exception = exception;
            IsCancelVisible = false;

            return Open();
        }

        public Task ShowFileBrowserDialog()
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            return ShowExceptionDialog(new NotImplementedException(Resources.DialogOpenAlready));
        }

        public Task ShowFolderBrowserDialog()
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            return ShowExceptionDialog(new NotImplementedException());
        }

        public Task ShowProgressDialog()
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            return ShowExceptionDialog(new NotImplementedException());
        }

        public async Task<List<Data.MediaItem>> ShowUrlParseDialog()
        {
            var result = new List<Data.MediaItem>();
            var viewmodel = new CreateMediaItem(_service, _mediaItemMapper);
            Context = viewmodel;
            Title = Resources.VideoAdd;

            AcceptAction = () =>
            {
                if (viewmodel.Result?.MediaItems?.Any() == true)
                    result.AddRange(viewmodel.Result.MediaItems);
            };

            await Open();

            return result;
        }

        public void Accept()
        {
            Close();
            AcceptAction?.Invoke();
        }

        public bool CanAccept()
        {
            return CanClose() && (CanAcceptFunc?.Invoke() ?? true) == true;
        }

        public void Cancel()
        {
            Close();
            CancelAction?.Invoke();
        }

        public bool CanCancel()
        {
            return CanClose() && (CanCancelFunc?.Invoke() ?? true) == true;
        }

        public async Task Open()
        {
            var tcs = new TaskCompletionSource<object>();
            EventHandler lambda = (s, e) => tcs.TrySetResult(null);
            try
            {
                DialogClosed += lambda;
                IsOpen = true; // open dialog
                await tcs.Task; // wait for dialog to close
            }
            finally
            {
                DialogClosed -= lambda;
            }
        }

        public void Close()
        {
            IsOpen = false;
        }

        public bool CanClose()
        {
            return IsOpen;
        }
    }
}
