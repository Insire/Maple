using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;
using SoftThorn.MonstercatNet;
using System;

namespace Maple
{
    public sealed class MonstercatReleaseViewModel : ViewModelBase, IMonstercatViewModel
    {
        private readonly IMonstercatApi _monstercatApi;

        public Guid Id { get; }
        public string Title { get; }

        public string ArtistsTitle { get; }
        public string GenrePrimary { get; }

        public string GenreSecondary { get; }

        public MonstercatReleaseViewModel(IMonstercatApi monstercatApi, IScarletCommandBuilder commandBuilder, Release model)
            : base(commandBuilder)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _monstercatApi = monstercatApi ?? throw new ArgumentNullException(nameof(monstercatApi));

            Id = model.Id;
            Title = model.Title ?? throw new ArgumentNullException(nameof(model.Title));

            ArtistsTitle = model.ArtistsTitle;
            GenrePrimary = model.GenrePrimary;
            GenreSecondary = model.GenreSecondary;
        }
    }
}
