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
        private readonly FileSystemViewModel _fileSystemViewModel;

        public PlaylistFactory(IScarletCommandBuilder commandBuilder, IValidator<Playlist> validator, FileSystemViewModel fileSystemViewModel)
        {
            _commandBuilder = commandBuilder ?? throw new ArgumentNullException(nameof(commandBuilder));
            _validator = validator;
            _fileSystemViewModel = fileSystemViewModel;
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
            return new CreatePlaylistViewModel(playlists, playlist, _validator, _fileSystemViewModel);
        }
    }
}
