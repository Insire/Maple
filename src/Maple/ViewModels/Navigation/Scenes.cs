﻿using System;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;

using Maple.Core;
using Maple.Domain;
using Maple.Localization.Properties;

namespace Maple
{
    /// <summary>
    /// ViewModel that stores and controls which UserControl(Page/View) whatever is displayed in the mainwindow of this app)
    /// </summary>
    /// <seealso cref="Maple.Core.BaseListViewModel{Maple.Scene}" />
    public class Scenes : BaseListViewModel<Scene>
    {
        private readonly ILocalizationService _manager;
        private readonly ILoggingService _log;

        private bool _isExpanded;
        /// <summary>
        /// Gets or sets a value indicating whether the control bound to this instance is expanded.
        /// </summary>
        /// <value>
        ///   <c>true</c> if this instance is expanded; otherwise, <c>false</c>.
        /// </value>
        /// <autogeneratedoc />
        public bool IsExpanded
        {
            get { return _isExpanded; }
            set { SetValue(ref _isExpanded, value); }
        }

        /// <summary>
        /// Gets the close expander command.
        /// </summary>
        /// <value>
        /// The close expander command.
        /// </value>
        /// <autogeneratedoc />
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
        /// Gets the open options page command.
        /// </summary>
        /// <value>
        /// The open options page command.
        /// </value>
        /// <autogeneratedoc />
        public ICommand OpenOptionsCommand { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Scenes"/> class.
        /// </summary>
        /// <param name="manager">The manager.</param>
        /// <param name="log">The log.</param>
        public Scenes(ILocalizationService manager, ILoggingService log, IMessenger messenger, FileSystemViewModel fileSystemViewModel)
            : base(messenger)
        {
            _manager = manager ?? throw new ArgumentNullException(nameof(manager), Resources.IsRequired);
            _log = log ?? throw new ArgumentNullException(nameof(log));

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

                // keeping this for debugging the FileBrowser
                //new Scene(_manager)
                //{
                //    Content = new ContentPresenter()
                //    {
                //        Content = new FileBrowserContentDialogViewModel(fileSystemViewModel, new FileSystemBrowserOptions()
                //        {
                //            CanCancel=false,
                //            MultiSelection= true,
                //            Title="Test"
                //        })
                //    },
                //    Key = nameof(Resources.Director),
                //    IsSelected = true,
                //    Sequence = 700,
                //},
            };

            using (BusyStack.GetToken())
            {
                AddRange(content);

                SelectedItem = this[0];
            }

            InitializeCommands();
        }

        private void InitializeCommands()
        {
            OpenColorOptionsCommand = new RelayCommand(OpenColorOptionsView, CanOpenColorOptionsView);
            OpenMediaPlayerCommand = new RelayCommand(OpenMediaPlayerView, CanOpenMediaPlayerView);
            OpenOptionsCommand = new RelayCommand(OpenOptionsView, CanOpenOptionsView);
            OpenGithubPageCommand = new RelayCommand(OpenGithubPage);
            CloseExpanderCommand = new RelayCommand(() => IsExpanded = false, () => IsExpanded != false);
        }

        private void OpenOptionsView()
        {
            SelectedItem = Items.First(p => p.Content.GetType() == typeof(OptionsPage));
        }

        private bool CanOpenOptionsView()
        {
            return Items?.Any(p => p.Content.GetType() == typeof(OptionsPage)) == true;
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
            return Items?.Any(p => p.Content.GetType() == typeof(MediaPlayerPage)) == true;
        }

        private void OpenGithubPage()
        {
            using (Process.Start(_manager.Translate(nameof(Resources.GithubProjectLink))))
            {
            }
        }
    }
}
