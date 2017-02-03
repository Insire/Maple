using Maple.Core;
using System;
using System.Windows.Input;

namespace Maple
{
    public class DialogViewModel : ObservableObject
    {
        public ExceptionDialogViewModel ExceptionDialogViewModel { get; private set; }
        public FileBrowserDialogViewModel FileBrowserDialogViewModel { get; private set; }
        public FolderBrowserDialogViewModel FolderBrowserDialogViewModel { get; private set; }
        public MessageDialogViewModel MessageDialogViewModel { get; private set; }
        public ProgressDialogViewModel ProgressDialogViewModel { get; private set; }

        public ICommand OpenDialogCommand { get; private set; }
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
            set { SetValue(ref _isOpen, value); }
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

        public DialogViewModel()
        {
            OpenDialogCommand = new RelayCommand(Open, () => CanOpen());
            CloseDialogCommand = new RelayCommand(Close, () => CanClose());
            CancelDialogCommand = new RelayCommand(Cancel, () => CanCancel());
            AcceptDialogCommand = new RelayCommand(Accept, () => CanAccept());

            ExceptionDialogViewModel = new ExceptionDialogViewModel();
            FileBrowserDialogViewModel = new FileBrowserDialogViewModel();
            FolderBrowserDialogViewModel = new FolderBrowserDialogViewModel();
            MessageDialogViewModel = new MessageDialogViewModel();
            ProgressDialogViewModel = new ProgressDialogViewModel();
        }

        public void ShowMessageDialog(string message, string title)
        {
            Context = MessageDialogViewModel;
            Title = title;
            MessageDialogViewModel.Message = message;
            IsCancelVisible = false;

            Open();
        }

        public void ShowExceptionDialog(Exception exception)
        {
            Context = ExceptionDialogViewModel;
            Title = exception.GetType().Name;
            ExceptionDialogViewModel.Exception = exception;
            IsCancelVisible = false;

            Open();
        }

        public void ShowFileBrowserDialog()
        {
            ShowExceptionDialog(new NotImplementedException());
        }

        public void ShowFolderBrowserDialog()
        {
            ShowExceptionDialog(new NotImplementedException());
        }

        public void ShowProgressDialog()
        {
            ShowExceptionDialog(new NotImplementedException());
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

        public void Open()
        {
            IsOpen = true;
        }

        public bool CanOpen()
        {
            return !IsOpen;
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
