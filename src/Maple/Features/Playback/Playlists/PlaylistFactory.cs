using System;
using FluentValidation;
using Maple.Domain;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Wpf.FileSystemBrowser;

namespace Maple
{
    public sealed class PlaylistFactory
    {
        private readonly IScarletCommandBuilder _commandBuilder;
        private readonly IValidator<Playlist> _validator;
        private readonly Func<FileSystemViewModel> _fileSystemViewModelFactory;

        public PlaylistFactory(IScarletCommandBuilder commandBuilder, IValidator<Playlist> validator, Func<FileSystemViewModel> fileSystemViewModelFactory)
        {
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
            _validator = validator ?? throw new ArgumentNullException(nameof(validator));
            _fileSystemViewModelFactory = fileSystemViewModelFactory ?? throw new ArgumentNullException(nameof(fileSystemViewModelFactory));
        }

        public Playlist Create(PlaylistModel model)
        {
            return new Playlist(_commandBuilder, model);
        }

        public CreatePlaylistViewModel Create(Playlists playlists)
        {
            var playerViewModel = new Playlist(new Playlist(_commandBuilder));

            return Create(playlists, playerViewModel);
        }

        public CreatePlaylistViewModel Create(Playlists playlists, Playlist playlist)
        {
            return new CreatePlaylistViewModel(playlists, playlist, _validator, _fileSystemViewModelFactory);
        }
    }
}
