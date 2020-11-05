using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;
using SoftThorn.MonstercatNet;
using System;

namespace Maple
{
    public sealed class MonstercatPlaylistViewModel : ViewModelListBase<MonstercatTrackViewModel>, IMonstercatViewModel
    {
        private readonly IMonstercatApi _monstercatApi;

        public Guid Id { get; }
        public string Title { get; }

        public int NumRecords { get; }
        public bool Public { get; }
        public bool MyLibrary { get; }

        public MonstercatPlaylistViewModel(IMonstercatApi monstercatApi, IScarletCommandBuilder commandBuilder, SoftThorn.MonstercatNet.Playlist model)
            : base(commandBuilder)
        {
            if (model is null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            _monstercatApi = monstercatApi ?? throw new ArgumentNullException(nameof(monstercatApi));

            Id = model.Id;
            Title = model.Name ?? throw new ArgumentNullException(nameof(model.Name));

            NumRecords = model.NumRecords;
            Public = model.Public;
            MyLibrary = model.MyLibrary;
        }
    }
}
