using Maple.Core;
using Maple.Localization.Properties;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

namespace Maple
{
    /// <summary>
    /// ViewModel that stores and controls which UserControl(Page/View) whatever is displayed in the mainwindow of this app)
    /// </summary>
    /// <seealso cref="Maple.Core.BaseListViewModel{Maple.Scene}" />
    public class Scenes : BaseListViewModel<Scene>
    {
        private readonly ITranslationService _manager;
        private readonly IMapleLog _log;

        private bool _isExpanded;
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetValue(ref _isExpanded, value); }
        }

        public ICommand CloseExpanderCommand { get; private set; }

        /// <summary>
        /// Gets the open color options command.
        /// </summary>
        /// <value>
        /// The open color options command.
        /// </value>
        public ICommand OpenColorOptionsCommand { get; private set; }
        /// <summary>
        /// Gets the open media player command.
        /// </summary>
        /// <value>
        /// The open media player command.
        /// </value>
        public ICommand OpenMediaPlayerCommand { get; private set; }
        /// <summary>
        /// Gets the open github page command.
        /// </summary>
        /// <value>
        /// The open github page command.
        /// </value>
        public ICommand OpenGithubPageCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenes"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="log">The log.</param>
        public Scenes(ITranslationService manager, IMapleLog log)
        {
            _manager = manager;
            _log = log;

            var content = new[]
            {
                new Scene(_manager)
                {
                    Content = new MediaPlayerPage(_manager),
                    Key = nameof(Resources.Playback),
                    IsSelected = true,
                    Sequence = 100,
                },

                new Scene(_manager)
                {
                    Content = new PlaylistsPage(_manager),
                    Key = nameof(Resources.Playlists),
                    IsSelected = false,
                    Sequence = 300,
                },

                new Scene(_manager)
                {
                    Content = new ColorOptionsPage(_manager),
                    Key = nameof(Resources.Themes),
                    IsSelected = false,
                    Sequence = 500,
                },

                new Scene(_manager)
                {
                    Content = new OptionsPage(_manager),
                    Key = nameof(Resources.Options),
                    IsSelected = false,
                    Sequence = 600,
                },

                new Scene(_manager)
                {
                    Content = new MediaPlayersPage(_manager),
                    Key = nameof(Resources.Director),
                    IsSelected = false,
                    Sequence = 150,
                },
            };

            using (BusyStack.GetToken())
            {
                AddRange(content);

                SelectedItem = Items[0];

                using (View.DeferRefresh())
                {
                    View.SortDescriptions.Add(new SortDescription(nameof(Scene.Sequence), ListSortDirection.Ascending));
                }
            }

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            OpenColorOptionsCommand = new RelayCommand(OpenColorOptionsView, CanOpenColorOptionsView);
            OpenMediaPlayerCommand = new RelayCommand(OpenMediaPlayerView, CanOpenMediaPlayerView);
            OpenGithubPageCommand = new RelayCommand(OpenGithubPage);
            CloseExpanderCommand = new RelayCommand(() => IsExpanded = false, () => IsExpanded != false);
        }

        private void OpenColorOptionsView()
        {
            SelectedItem = Items.First(p => p.Content.GetType() == typeof(ColorOptionsPage));
        }

        private bool CanOpenColorOptionsView()
        {
            return Items?.Any(p => p.Content.GetType() == typeof(ColorOptionsPage)) == true;
        }

        private void OpenMediaPlayerView()
        {
            SelectedItem = Items.First(p => p.Content.GetType() == typeof(MediaPlayerPage));
        }

        private bool CanOpenMediaPlayerView()
        {
            return Items?.Any(p => p.Content.GetType() == typeof(ColorOptionsPage)) == true;
        }

        private void OpenGithubPage()
        {
            Process.Start(_manager.Translate(nameof(Resources.GithubProjectLink)));
        }
    }
}
