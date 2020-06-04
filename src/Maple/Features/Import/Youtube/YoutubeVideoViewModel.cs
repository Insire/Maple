using Maple.Youtube;

namespace Maple
{
    public sealed class YoutubeVideoViewModel : YoutubeRessourceViewModel
    {
        public long Duration { get; }

        public YoutubeVideoViewModel(YoutubeVideo model)
            : base(model)
        {
            Duration = model.Duration;
        }
    }
}
