using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using Maple.Properties;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Commands;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    internal sealed class NavigationViewModel : Scenes
    {
        private readonly LocalizationsViewModel _localizationsViewModel;

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetValue(ref _isExpanded, value); }
        }

        public ICommand CloseExpanderCommand { get; }
        public ICommand OpenMediaPlayerCommand { get; }
        public ICommand OpenGithubPageCommand { get; }
        public ICommand OpenOptionsCommand { get; }

        public NavigationViewModel(IScarletCommandBuilder commandBuilder, PlaybackViewModel playback, LocalizationsViewModel localizationsViewModel, MediaPlayers mediaPlayers, Playlists playlists, OptionsViewModel options)
            : base(commandBuilder, localizationsViewModel)
        {
            _localizationsViewModel = localizationsViewModel ?? throw new ArgumentNullException(nameof(localizationsViewModel));

            Add(nameof(Resources.Playback), playback);
            Add(nameof(Resources.MediaPlayers), mediaPlayers);
            Add(nameof(Resources.Playlists), playlists);
            Add(nameof(Resources.Options), options);

            SelectedItem = this[0];

            OpenMediaPlayerCommand = new RelayCommand(CommandManager, OpenMediaPlayerView, CanOpenMediaPlayerView);
            OpenOptionsCommand = new RelayCommand(CommandManager, OpenOptionsView, CanOpenOptionsView);
            OpenGithubPageCommand = new RelayCommand(CommandManager, OpenGithubPage);
            CloseExpanderCommand = new RelayCommand(CommandManager, () => IsExpanded = false, () => IsExpanded != false);
        }

        private void OpenOptionsView()
        {
            SelectedItem = Items.First(p => p.Content is OptionsViewModel);
        }

        private bool CanOpenOptionsView()
        {
            return Items?.Any(p => p.Content is OptionsViewModel) == true;
        }

        private void OpenMediaPlayerView()
        {
            SelectedItem = Items.First(p => p.Content is MediaPlayers);
        }

        private bool CanOpenMediaPlayerView()
        {
            return Items?.Any(p => p.Content is MediaPlayers) == true;
        }

        private void OpenGithubPage()
        {
            using (Process.Start(_localizationsViewModel.Translate(nameof(Resources.GithubProjectLink))))
            {
            }
        }
    }
}
