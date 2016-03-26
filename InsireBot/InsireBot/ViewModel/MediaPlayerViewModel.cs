using System;
using GalaSoft.MvvmLight.Messaging;
using InsireBot.MediaPlayer;

namespace InsireBot.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MediaPlayerViewModel : BotViewModelBase<MediaItem>, IPlaying
    {
        public IMediaPlayer<IMediaItem> MediaPlayer { get; }

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MediaPlayerViewModel()
        {
            if (IsInDesignMode)
            {
                var item = new MediaItem("test", "");

                Items.Add(item);
            }
            else
            {
                // Code runs "for real"
                //MediaPlayer = MediaPlayerFactory.Create(MediaPlayerType.VLCDOTNET);
            }
        }

        public void Next()
        {
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Next,
            };

            Messenger.Default.Send(contract);
        }

        public void Previous()
        {
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Previous,
            };

            Messenger.Default.Send(contract);
        }

        public void Play()
        {
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Play,
            };

            Messenger.Default.Send(contract);
        }

        public void Pause()
        {
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Pause,
            };

            Messenger.Default.Send(contract);
        }

        public void Stop()
        {
            var contract = new MediaItemContract
            {
                MediaItem = SelectedItem,
                Action = MediaItemAction.Stop,
            };

            Messenger.Default.Send(contract);
        }
    }
}