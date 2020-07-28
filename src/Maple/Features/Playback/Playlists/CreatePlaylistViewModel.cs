using System;
using System.Collections.ObjectModel;
using FluentValidation;
using FluentValidation.Results;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;
using MvvmScarletToolkit.Wpf.FileSystemBrowser;

namespace Maple
{
    public sealed class CreatePlaylistViewModel : ObservableObject
    {
        private readonly ObservableCollection<ValidationFailure> _errors;
        public Playlists Playlists { get; }
        public Playlist Playlist { get; }

        public IValidator<Playlist> PlaylistValidator { get; }
        public FileSystemViewModel FileSystemViewModel { get; }
        public ReadOnlyObservableCollection<ValidationFailure> Errors { get; }

        private ValidationFailure _error;
        public ValidationFailure Error
        {
            get { return _error; }
            set { SetValue(ref _error, value); }
        }

        public CreatePlaylistViewModel(Playlists playlists, Playlist playlist, IValidator<Playlist> playlistValidator, Func<FileSystemViewModel> fileSystemViewModelFactory)
        {
            Playlists = playlists ?? throw new ArgumentNullException(nameof(playlists));
            Playlist = playlist ?? throw new ArgumentNullException(nameof(playlist));
            PlaylistValidator = playlistValidator ?? throw new ArgumentNullException(nameof(playlistValidator));

            FileSystemViewModel = fileSystemViewModelFactory();

            _errors = new ObservableCollection<ValidationFailure>();
            Errors = new ReadOnlyObservableCollection<ValidationFailure>(_errors);
        }

        public bool CanSave()
        {
            var result = PlaylistValidator.Validate(Playlist);

            _errors.UpdateItems(result.Errors, Comparer, Mapper);

            if (!result.IsValid)
            {
                Error = _errors[0];
            }
            else
            {
                Error = null;
            }

            return result.IsValid;
        }

        private bool Comparer(ValidationFailure @old, ValidationFailure @new)
        {
            return old.PropertyName == @new.PropertyName
                && old.ErrorMessage == @new.ErrorMessage;
        }

        private ValidationFailure Mapper(ValidationFailure list)
        {
            return list;
        }
    }
}
