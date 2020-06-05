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
        public ICommand OpenMediaPlayerCommand { get; }
        public ICommand OpenOptionsCommand { get; }

        public NavigationViewModel(IScarletCommandBuilder commandBuilder,
            PlaybackViewModel playback,
            LocalizationsViewModel localizations,
            MediaPlayers mediaPlayers,
            Playlists playlists,
            OptionsViewModel options,
            DashboardViewModel dashboard,
            AboutViewModel about)
            : base(commandBuilder, localizations)
        {
            Add(nameof(Resources.Dashboard), dashboard);
            Add(nameof(Resources.Playback), playback);
            Add(nameof(Resources.MediaLibrary), playlists);
            Add(nameof(Resources.MediaPlayers), mediaPlayers);
            Add(nameof(Resources.Options), options);
            Add(nameof(Resources.About), about);

            SelectedItem = this[0];

            OpenMediaPlayerCommand = new RelayCommand(CommandManager, OpenMediaPlayerView, CanOpenMediaPlayerView);
            OpenOptionsCommand = new RelayCommand(CommandManager, OpenOptionsView, CanOpenOptionsView);
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
    }
}
