using System;
using GalaSoft.MvvmLight.Messaging;

namespace InsireBot.MediaPlayer
{
    public class MediaItemFactory
    {
        public void Create(string url)
        {
            Create(new Uri(url));
        }

        public void Create(Uri url)
        {
            var item = new MediaItem("","");

            // parse the url

            Messenger.Default.Send(item);
        }
    }
}
