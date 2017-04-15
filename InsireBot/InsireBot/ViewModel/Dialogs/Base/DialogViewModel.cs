using Maple.Core;
using Maple.Localization.Properties;
using Maple.Youtube;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Maple.Core.ObservableObject" />
    public class DialogViewModel : ObservableObject
    {
        private readonly IYoutubeUrlParseService _service;
        private readonly IMediaItemMapper _mediaItemMapper;
        private readonly FileSystemViewModel _fileSystemViewModel;
        /// <summary>
        /// The dialog closed
        /// </summary>
        public EventHandler DialogClosed;

        /// <summary>
        /// Gets the exception dialog view model.
        /// </summary>
        /// <value>
        /// The exception dialog view model.
        /// </value>
        public ExceptionDialogViewModel ExceptionDialogViewModel { get; private set; }
        /// <summary>
        /// Gets the message dialog view model.
        /// </summary>
        /// <value>
        /// The message dialog view model.
        /// </value>
        public MessageDialogViewModel MessageDialogViewModel { get; private set; }
        /// <summary>
        /// Gets the progress dialog view model.
        /// </summary>
        /// <value>
        /// The progress dialog view model.
        /// </value>
        public ProgressDialogViewModel ProgressDialogViewModel { get; private set; }

        /// <summary>
        /// Gets the close dialog command.
        /// </summary>
        /// <value>
        /// The close dialog command.
        /// </value>
        public ICommand CloseDialogCommand { get; private set; }
        /// <summary>
        /// Gets the cancel dialog command.
        /// </summary>
        /// <value>
        /// The cancel dialog command.
        /// </value>
        public ICommand CancelDialogCommand { get; private set; }
        /// <summary>
        /// Gets the accept dialog command.
        /// </summary>
        /// <value>
        /// The accept dialog command.
        /// </value>
        public ICommand AcceptDialogCommand { get; private set; }

        /// <summary>
        /// Gets or sets the accept action.
        /// </summary>
        /// <value>
        /// The accept action.
        /// </value>
        public Action AcceptAction { get; set; }
        /// <summary>
        /// Gets or sets the cancel action.
        /// </summary>
        /// <value>
        /// The cancel action.
        /// </value>
        public Action CancelAction { get; set; }
        /// <summary>
        /// Gets or sets the can cancel function.
        /// </summary>
        /// <value>
        /// The can cancel function.
        /// </value>
        public Func<bool> CanCancelFunc { get; set; }
        /// <summary>
        /// Gets or sets the can accept function.
        /// </summary>
        /// <value>
        /// The can accept function.
        /// </value>
        public Func<bool> CanAcceptFunc { get; set; }

        private bool _isOpen;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is open.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is open; otherwise, <c>false</c>.
        /// </value>
        public bool IsOpen
        {
            get { return _isOpen; }
            set { SetValue(ref _isOpen, value, OnChanged: OnOpenChanged); }
        }

        private bool _isCancelVisible;
        /// <summary>
        /// Gets or sets a value indicating whether this instance is cancel visible.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is cancel visible; otherwise, <c>false</c>.
        /// </value>
        public bool IsCancelVisible
        {
            get { return _isCancelVisible; }
            set { SetValue(ref _isCancelVisible, value); }
        }

        private string _title;
        /// <summary>
        /// Gets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title
        {
            get { return _title; }
            private set { SetValue(ref _title, value); }
        }

        private ObservableObject _context;
        /// <summary>
        /// Gets or sets the context.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public ObservableObject Context
        {
            get { return _context; }
            set { SetValue(ref _context, value); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DialogViewModel"/> class.
        /// </summary>
        /// <param name="service">The service.</param>
        /// <param name="mediaItemMapper">The media item mapper.</param>
        public DialogViewModel(IYoutubeUrlParseService service, IMediaItemMapper mediaItemMapper, FileSystemViewModel fileSystemViewModel)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper));
            _fileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel));

            CloseDialogCommand = new RelayCommand(Close, () => CanClose());
            CancelDialogCommand = new RelayCommand(Cancel, () => CanCancel());
            AcceptDialogCommand = new RelayCommand(Accept, () => CanAccept());

            ExceptionDialogViewModel = new ExceptionDialogViewModel();
            MessageDialogViewModel = new MessageDialogViewModel();
            ProgressDialogViewModel = new ProgressDialogViewModel();
        }

        private void OnOpenChanged()
        {
            if (!IsOpen)
                DialogClosed?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Shows the message dialog.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="title">The title.</param>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
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

        /// <summary>
        /// Shows the exception dialog.
        /// </summary>
        /// <param name="exception">The exception.</param>
        /// <returns></returns>
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

        /// <summary>
        /// Shows the file browser dialog.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public async Task<(DialogResult Result, IList<IFileSystemFile> Files)> ShowFileBrowserDialog(FileSystemBrowserOptions options)
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            var tuple = default((DialogResult Result, IList<IFileSystemFile> Files));
            var viewModel = new FileBrowserDialogViewModel(_fileSystemViewModel, options);

            Context = viewModel;
            Title = options.Title;
            IsCancelVisible = options.CanCancel;

            AcceptAction = () =>
            {
                var items = viewModel.FileSystemViewModel.SelectedItems;
                tuple = (DialogResult.OK, items.Select(p => p as IFileSystemFile).Where(p => p != null).ToList());
            };

            CancelAction = () =>
            {
                var items = viewModel.FileSystemViewModel.SelectedItems;
                tuple = (DialogResult.Cancel, new List<IFileSystemFile>());
            };

            await Open();

            return tuple;
        }

        /// <summary>
        /// Shows the folder browser dialog.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public async Task<(DialogResult Result, IFileSystemDirectory Directory)> ShowFolderBrowserDialog(FileSystemBrowserOptions options)
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            var tuple = default((DialogResult Result, IFileSystemDirectory Directory));
            var viewModel = new FileBrowserDialogViewModel(_fileSystemViewModel, options);

            Context = viewModel;
            Title = options.Title;
            IsCancelVisible = options.CanCancel;

            AcceptAction = () =>
            {
                var items = viewModel.FileSystemViewModel.SelectedItems;
                tuple = (DialogResult.OK, items.FirstOrDefault() as IFileSystemDirectory);
            };

            CancelAction = () =>
            {
                var items = viewModel.FileSystemViewModel.SelectedItems;
                tuple = (DialogResult.Cancel, default(IFileSystemDirectory));
            };

            await Open();

            return tuple;
        }

        /// <summary>
        /// Shows the progress dialog.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public Task ShowProgressDialog()
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            return ShowExceptionDialog(new NotImplementedException());
        }

        /// <summary>
        /// Shows the URL parse dialog.
        /// </summary>
        /// <returns></returns>
        public async Task<List<Data.MediaItem>> ShowUrlParseDialog()
        {
            var result = new List<Data.MediaItem>();
            var viewmodel = new CreateMediaItem(_service, _mediaItemMapper);
            Context = viewmodel;
            Title = Resources.VideoAdd;

            AcceptAction = () =>
            {
                if (viewmodel.Result?.MediaItems?.Any() == true)
                {
                    var items = _mediaItemMapper.GetManyData(viewmodel.Result.MediaItems);
                    result.AddRange(items);
                }
            };

            await Open();

            return result;
        }

        /// <summary>
        /// Accepts this instance.
        /// </summary>
        public void Accept()
        {
            Close();
            AcceptAction?.Invoke();
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
            Close();
            CancelAction?.Invoke();
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

        /// <summary>
        /// Opens this instance.
        /// </summary>
        /// <returns></returns>
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
