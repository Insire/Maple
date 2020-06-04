using Maple.Domain;
using Maple.Youtube;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public abstract class YoutubeRessourceViewModel : ObservableObject
    {
        public string Title { get; }

        public string Description { get; }

        public string Location { get; }

        public PrivacyStatus PrivacyStatus { get; }

        public YoutubeRessourceViewModel(YoutubeRessource model)
        {
            Title = model.Title;
            Description = model.Description;
            Location = model.Location;
            PrivacyStatus = (PrivacyStatus)model.PrivacyStatus;
        }
    }
}
