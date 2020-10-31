using SoftThorn.MonstercatNet;

namespace Maple
{
    public sealed class MonstercatReleaseViewModel : MonstercatRessourceViewModel
    {
        public string ArtistsTitle { get; }
        public string GenrePrimary { get; }

        public string GenreSecondary { get; }

        public MonstercatReleaseViewModel(Release model)
            : base(model)
        {
            ArtistsTitle = model.ArtistsTitle;
            GenrePrimary = model.GenrePrimary;
            GenreSecondary = model.GenreSecondary;
        }
    }
}
