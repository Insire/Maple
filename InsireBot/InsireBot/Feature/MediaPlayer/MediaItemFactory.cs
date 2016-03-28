using GalaSoft.MvvmLight.Messaging;

namespace InsireBot.MediaPlayer
{
    public class MediaItemFactory
    {
        public static async void Create(string url)
        {
            var yt = new Youtube();

            var videos = await yt.GetVideo(url);

            foreach (var video in videos)
                Messenger.Default.Send(video);
        }
    }
}
