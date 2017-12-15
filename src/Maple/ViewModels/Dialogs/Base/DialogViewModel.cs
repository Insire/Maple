using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Core;
using Maple.Localization.Properties;
using Maple.Youtube;

namespace Maple
{
    public class DialogViewModel : ObservableObject, IDialogViewModel
    {
        private readonly IMessenger _messenger;
        private readonly ILocalizationService _translator;
        private readonly IYoutubeUrlParser _service;
        private readonly IMediaItemMapper _mediaItemMapper;
        private readonly FileSystemViewModel _fileSystemViewModel;

        private readonly Func<CreateMediaItem> _createMediaItemFactory;

        public EventHandler DialogClosed;

        public ExceptionDialogViewModel ExceptionDialogViewModel { get; private set; }
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

        private string _titleDetail;
        public string TitleDetail
        {
            get { return _titleDetail; }
            private set { SetValue(ref _titleDetail, value); }
        }

        private ObservableObject _context;
        public ObservableObject Context
        {
            get { return _context; }
            set { SetValue(ref _context, value); }
        }

        public DialogViewModel(ILocalizationService translator, IYoutubeUrlParser service, IMediaItemMapper mediaItemMapper, IMessenger messenger, FileSystemViewModel fileSystemViewModel, Func<CreateMediaItem> createMediaItemFactory)
        {
            _translator = translator ?? throw new ArgumentNullException(nameof(translator), $"{nameof(translator)} {Resources.IsRequired}");
            _service = service ?? throw new ArgumentNullException(nameof(service), $"{nameof(service)} {Resources.IsRequired}");
            _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper), $"{nameof(mediaItemMapper)} {Resources.IsRequired}");
            _fileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel), $"{nameof(fileSystemViewModel)} {Resources.IsRequired}");
            _messenger = messenger ?? throw new ArgumentNullException(nameof(messenger), $"{nameof(messenger)} {Resources.IsRequired}");
            _createMediaItemFactory = createMediaItemFactory ?? throw new ArgumentNullException(nameof(createMediaItemFactory), $"{nameof(createMediaItemFactory)} {Resources.IsRequired}");


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

            TitleDetail = string.Empty;
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
                return Task.CompletedTask;

            TitleDetail = string.Empty;
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
        public async Task<(bool Result, IList<IFileSystemFile> Files)> ShowFileBrowserDialog(FileSystemBrowserOptions options, CancellationToken token)
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            var tuple = default((bool Result, IList<IFileSystemFile> Files));
            var viewModel = new FileBrowserDialogViewModel(_fileSystemViewModel, options);

            TitleDetail = string.Empty;
            Context = viewModel;
            Title = options.Title;
            IsCancelVisible = options.CanCancel;

            using (_messenger.Subscribe<FileSystemInfoChangedMessage>(FileSystemInfoChanged))
            {
                AcceptAction = () =>
                {
                    var items = viewModel.FileSystemViewModel.SelectedItems;
                    tuple = (true, items.Select(p => p as IFileSystemFile).Where(p => p != null).ToList());
                };

                CancelAction = () =>
                {
                    var items = viewModel.FileSystemViewModel.SelectedItems;
                    tuple = (false, new List<IFileSystemFile>());
                };

                await Open(token).ConfigureAwait(false);
            }

            return tuple;
        }

        public async Task<(bool Result, ICollection<MediaItem> MediaItems)> ShowMediaItemSelectionDialog(FileSystemBrowserOptions options, CancellationToken token)
        {
            var mediaItems = new List<MediaItem>();
            (var Result, var Files) = await ShowFileBrowserDialog(options, token).ConfigureAwait(true);

            if (!Result)
                return (Result, mediaItems);

            foreach (var file in Files)
            {
                // TODO parse the files and generate mediaitems from them
            }

            return (Result, mediaItems);
        }

        public async Task<(bool Result, ICollection<MediaItem> MediaItems)> ShowMediaItemFolderSelectionDialog(FileSystemBrowserOptions options, CancellationToken token)
        {
            var mediaItems = new List<MediaItem>();
            (var Result, var Folder) = await ShowFolderBrowserDialog(options, token).ConfigureAwait(true);

            if (!Result)
                return (Result, mediaItems);

            foreach (var file in Folder.Children)
            {
                // TODO get the files from the folder
                // TODO parse the files from the folder and generate mediaitems from them
            }

            return (Result, mediaItems);
        }

        /// <summary>
        /// Shows the folder browser dialog.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.InvalidOperationException"></exception>
        public async Task<(bool Result, IFileSystemDirectory Directory)> ShowFolderBrowserDialog(FileSystemBrowserOptions options, CancellationToken token)
        {
            if (IsOpen)
                throw new InvalidOperationException(Resources.DialogOpenAlready);

            var tuple = default((bool Result, IFileSystemDirectory Directory));
            var viewModel = new FileBrowserDialogViewModel(_fileSystemViewModel, options);

            TitleDetail = string.Empty;
            Context = viewModel;
            Title = options.Title;
            IsCancelVisible = options.CanCancel;

            using (_messenger.Subscribe<FileSystemInfoChangedMessage>(FileSystemInfoChanged))
            {
                AcceptAction = () =>
                {
                    var items = viewModel.FileSystemViewModel.SelectedItems;
                    items.Add(viewModel.FileSystemViewModel.SelectedItem);
                    tuple = (true, items.Distinct().FirstOrDefault() as IFileSystemDirectory);
                };

                CancelAction = () =>
                {
                    var items = viewModel.FileSystemViewModel.SelectedItems;
                    tuple = (false, default(IFileSystemDirectory));
                };

                await Open(token).ConfigureAwait(false);
            }

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
        public async Task<ICollection<MediaItem>> ShowUrlParseDialog(CancellationToken token)
        {
            var result = new List<MediaItem>();
            var viewmodel = new CreateMediaItem(_service, _mediaItemMapper, _messenger);

            TitleDetail = string.Empty;
            Context = viewmodel;
            Title = _translator.Translate(nameof(Resources.VideoAdd));

            AcceptAction = () =>
            {
                if (viewmodel.Result?.MediaItems?.Any() == true)
                {
                    var items = _mediaItemMapper.GetMany(viewmodel.Result.MediaItems);
                    result.AddRange(items);
                }
            };

            await Open(token).ConfigureAwait(false);

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

        private void FileSystemInfoChanged(FileSystemInfoChangedMessage e)
        {
            TitleDetail = e.Content.FullName;
        }
    }
}
