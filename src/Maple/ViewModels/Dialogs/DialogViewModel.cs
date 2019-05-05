using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

using Maple.Core;
using Maple.Localization.Properties;
using Maple.Youtube;

namespace Maple
{
    public class DialogViewModel : DialogBaseViewModel, IDialogViewModel
    {
        private readonly ILocalizationService _translator;
        private readonly IYoutubeUrlParser _service;
        private readonly IMediaItemMapper _mediaItemMapper;
        private readonly FileSystemViewModel _fileSystemViewModel;

        private readonly Func<CreateMediaItem> _createMediaItemFactory;

        public DialogViewModel(ILocalizationService translator, IYoutubeUrlParser service, IMediaItemMapper mediaItemMapper, IMessenger messenger, FileSystemViewModel fileSystemViewModel, Func<CreateMediaItem> createMediaItemFactory)
            : base(messenger)
        {
            _translator = translator ?? throw new ArgumentNullException(nameof(translator), $"{nameof(translator)} {Resources.IsRequired}");
            _service = service ?? throw new ArgumentNullException(nameof(service), $"{nameof(service)} {Resources.IsRequired}");
            _mediaItemMapper = mediaItemMapper ?? throw new ArgumentNullException(nameof(mediaItemMapper), $"{nameof(mediaItemMapper)} {Resources.IsRequired}");
            _fileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel), $"{nameof(fileSystemViewModel)} {Resources.IsRequired}");
            _createMediaItemFactory = createMediaItemFactory ?? throw new ArgumentNullException(nameof(createMediaItemFactory), $"{nameof(createMediaItemFactory)} {Resources.IsRequired}");

            CloseDialogCommand = new RelayCommand(Close, () => CanClose());
            CancelDialogCommand = new RelayCommand(Cancel, () => CanCancel());
            AcceptDialogCommand = new RelayCommand(Accept, () => CanAccept());

            ExceptionDialogViewModel = new ExceptionContentDialogViewModel();
            MessageDialogViewModel = new MessageContentDialogViewModel();
            ProgressDialogViewModel = new ProgressContentDialogViewModel();
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
            var viewModel = new FileBrowserContentDialogViewModel(_fileSystemViewModel, options);

            TitleDetail = string.Empty;
            Context = viewModel;
            Title = options.Title;
            IsCancelVisible = options.CanCancel;

            using (Messenger.Subscribe<FileSystemInfoChangedMessage>(FileSystemInfoChanged))
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

        public async Task<(bool Result, ICollection<MediaItem> MediaItems)> ShowMediaItemFolderSelectionDialog(FileSystemFolderBrowserOptions options, CancellationToken token)
        {
            var mediaItems = new List<MediaItem>();
            (var Result, var Folder) = await ShowFolderBrowserDialog(options, token).ConfigureAwait(true);

            if (!Result)
                return (Result, mediaItems);

            // should handle items depending on options

            IEnumerable<IFileSystemInfo> query = Folder.Children;

            if (options.IncludeSubFolders)
            {
                // TODO
            }

            var supportedFileExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
            {
                ".flac",
                ".mp3"
            };

            foreach (var file in query.Where(p => !p.IsContainer && supportedFileExtensions.Contains(Path.GetExtension(p.Name))))
            {
                var factory = _createMediaItemFactory();

                mediaItems.Add(factory.Create(file.FullName));
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
            var viewModel = new FileBrowserContentDialogViewModel(_fileSystemViewModel, options);

            TitleDetail = string.Empty;
            Context = viewModel;
            Title = options.Title;
            IsCancelVisible = options.CanCancel;

            using (Messenger.Subscribe<FileSystemInfoChangedMessage>(FileSystemInfoChanged))
            {
                AcceptAction = () =>
                {
                    if (viewModel.FileSystemViewModel.SelectedItem is IFileSystemDirectory directory)
                    {
                        tuple = (true, directory);
                    }
                    else
                    {
                        tuple = (false, default(IFileSystemDirectory));
                    }
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
            var viewmodel = new CreateMediaItem(_service, _mediaItemMapper, Messenger);

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

        public Task ShowMediaPlayerConfiguration(IMediaPlayersViewModel mediaPlayersViewModel)
        {
            if (IsOpen) // no exception spam, could probably be improved TODO ?
                return Task.CompletedTask;

            TitleDetail = string.Empty;
            Context = mediaPlayersViewModel;
            Title = _translator.Translate(nameof(Resources.Director));
            IsCancelVisible = false;

            return Open();
        }

        private void FileSystemInfoChanged(FileSystemInfoChangedMessage e)
        {
            TitleDetail = e.Content.FullName;
        }
    }
}
