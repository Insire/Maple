using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Maple.Localization.Properties;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Commands;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class NavigationViewModel : Scenes
    {
        private readonly ILocalizationService _manager;
        private readonly DialogViewModel _dialogViewModel;

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetValue(ref _isExpanded, value); }
        }

        public ICommand CloseExpanderCommand { get; }
        public ICommand OpenColorOptionsCommand { get; }
        public ICommand OpenMediaPlayerCommand { get; }
        public ICommand OpenGithubPageCommand { get; }
        public ICommand OpenOptionsCommand { get; }

        public ICommand OpenMediaPlayerConfigurationCommand { get; }

        public NavigationViewModel(ICommandBuilder commandBuilder, ILocalizationService localizationService, DialogViewModel dialogViewModel)
            : base(commandBuilder)
        {
            _manager = localizationService ?? throw new ArgumentNullException(nameof(localizationService));
            _dialogViewModel = dialogViewModel ?? throw new ArgumentNullException(nameof(dialogViewModel));

            _items.Add(new Scene(commandBuilder, new LocalizationViewModel(localizationService, nameof(Resources.Playback)))
            {
                Content = new MediaPlayerPage(_manager),
                IsSelected = true,
                Sequence = 100,
            });

            _items.Add(new Scene(commandBuilder, new LocalizationViewModel(localizationService, nameof(Resources.Playlists)))
            {
                Content = new PlaylistsPage(_manager),
                IsSelected = true,
                Sequence = 200,
            });

            _items.Add(new Scene(commandBuilder, new LocalizationViewModel(localizationService, nameof(Resources.Themes)))
            {
                Content = new ColorOptionsPage(_manager),
                IsSelected = true,
                Sequence = 300,
            });

            _items.Add(new Scene(commandBuilder, new LocalizationViewModel(localizationService, nameof(Resources.Options)))
            {
                Content = new OptionsPage(_manager),
                IsSelected = true,
                Sequence = 400,
            });

            SelectedItem = this[0];

            OpenColorOptionsCommand = new RelayCommand(OpenColorOptionsView, CanOpenColorOptionsView);
            OpenMediaPlayerCommand = new RelayCommand(OpenMediaPlayerView, CanOpenMediaPlayerView);
            OpenOptionsCommand = new RelayCommand(OpenOptionsView, CanOpenOptionsView);
            OpenGithubPageCommand = new RelayCommand(OpenGithubPage);
            CloseExpanderCommand = new RelayCommand(() => IsExpanded = false, () => IsExpanded != false);
            OpenMediaPlayerConfigurationCommand = CommandBuilder
                .Create(OpenMediaPlayerConfiguration)
                .WithSingleExecution(CommandManager)
                .Build();
        }

        private void OpenOptionsView()
        {
            SelectedItem = Items.First(p => p.Content is OptionsPage);
        }

        private bool CanOpenOptionsView()
        {
            return Items?.Any(p => p.Content is OptionsPage) == true;
        }

        private void OpenColorOptionsView()
        {
            SelectedItem = Items.First(p => p.Content is ColorOptionsPage);
        }

        private bool CanOpenColorOptionsView()
        {
            return Items?.Any(p => p.Content is ColorOptionsPage) == true;
        }

        private void OpenMediaPlayerView()
        {
            SelectedItem = Items.First(p => p.Content is MediaPlayerPage);
        }

        private bool CanOpenMediaPlayerView()
        {
            return Items?.Any(p => p.Content is MediaPlayerPage) == true;
        }

        private Task OpenMediaPlayerConfiguration()
        {
            return _dialogViewModel.ShowMediaPlayerConfiguration();
        }

        private void OpenGithubPage()
        {
            using (Process.Start(_manager.Translate(nameof(Resources.GithubProjectLink))))
            {
            }
        }
    }
}
