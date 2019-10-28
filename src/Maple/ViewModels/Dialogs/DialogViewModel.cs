using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Maple.Domain;
using Maple.Properties;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Commands;
using MvvmScarletToolkit.FileSystemBrowser;

namespace Maple
{
    public sealed class DialogViewModel : DialogBaseViewModel
    {
        private readonly IScarletMessenger _messenger;
        private readonly ILocalizationService _translator;
        private readonly FileSystemViewModel _fileSystemViewModel;
        private readonly YoutubeImportViewModel _youtubeImport;

        public DialogViewModel(IMapleCommandBuilder commandBuilder, FileSystemViewModel fileSystemViewModel, YoutubeImportViewModel youtubeImport)
            : base(commandBuilder)
        {
            _translator = commandBuilder.LocalizationService;
            _messenger = commandBuilder.Messenger;

            _youtubeImport = youtubeImport ?? throw new ArgumentNullException(nameof(youtubeImport));
            _fileSystemViewModel = fileSystemViewModel ?? throw new ArgumentNullException(nameof(fileSystemViewModel));

            CloseDialogCommand = new RelayCommand(CommandManager, Close, () => CanClose());
            CancelDialogCommand = new RelayCommand(CommandManager, Cancel, () => CanCancel());
            AcceptDialogCommand = new RelayCommand(CommandManager, Accept, () => CanAccept());

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
            Title = exception.Message;
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

            using (_messenger.Subscribe<FileSystemInfoChangedMessage>(FileSystemInfoChanged))
            {
                AcceptAction = () =>
                {
                    var items = viewModel.FileSystemViewModel.SelectedItems;
                    tuple = (true, items.Cast<IFileSystemFile>().Where(p => !(p is null)).ToList());
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

        public async Task<(bool Result, ICollection<MediaItemModel> MediaItems)> ShowMediaItemSelectionDialog(FileSystemBrowserOptions options, CancellationToken token)
        {
            var mediaItems = new List<MediaItemModel>();
            (var Result, var Files) = await ShowFileBrowserDialog(options, token).ConfigureAwait(true);

            if (!Result)
                return (Result, mediaItems);

            foreach (var file in Files)
            {
                // TODO parse the files and generate mediaitems from them
            }

            return (Result, mediaItems);
        }

        public async Task<(bool Result, ICollection<MediaItemModel> MediaItems)> ShowMediaItemFolderSelectionDialog(FileSystemFolderBrowserOptions options, CancellationToken token)
        {
            var mediaItems = default(ICollection<MediaItemModel>);
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

            mediaItems = query
                .Where(p => !p.IsContainer && supportedFileExtensions.Contains(Path.GetExtension(p.Name)))
                .Select(p => new MediaItemModel())
                .ToList();

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

            using (_messenger.Subscribe<FileSystemInfoChangedMessage>(FileSystemInfoChanged))
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

        public async Task<ICollection<YoutubeVideoViewModel>> ShowYoutubeVideoImport(CancellationToken token)
        {
            var result = default(ICollection<YoutubeVideoViewModel>);

            TitleDetail = string.Empty;
            Context = _youtubeImport;
            Title = _translator.Translate(nameof(Resources.VideoAdd));

            AcceptAction = () =>
            {
                result = _youtubeImport.Items.Where(p => p is YoutubeVideoViewModel).Cast<YoutubeVideoViewModel>().ToList();
            };

            await Open(token).ConfigureAwait(false);

            return result;
        }

        public async Task<ICollection<YoutubePlaylistViewModel>> ShowYoutubePlaylistImport(CancellationToken token)
        {
            var result = default(ICollection<YoutubePlaylistViewModel>);

            TitleDetail = string.Empty;
            Context = _youtubeImport;
            Title = _translator.Translate(nameof(Resources.PlaylistAdd));

            AcceptAction = () =>
            {
                result = _youtubeImport.Items.Where(p => p is YoutubePlaylistViewModel).Cast<YoutubePlaylistViewModel>().ToList();
            };

            await Open(token).ConfigureAwait(false);

            return result;
        }

        private void FileSystemInfoChanged(FileSystemInfoChangedMessage e)
        {
            TitleDetail = e.Content.FullName;
        }
    }
}
