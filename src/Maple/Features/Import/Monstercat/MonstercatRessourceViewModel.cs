using MvvmScarletToolkit.Observables;
using SoftThorn.MonstercatNet;
using System;

namespace Maple
{
    public abstract class MonstercatRessourceViewModel : ObservableObject
    {
        public Guid Id { get; }

        public string Title { get; }

        public MonstercatRessourceViewModel(SoftThorn.MonstercatNet.Playlist model)
        {
            Title = model.Name;
            Id = model.Id;
        }

        public MonstercatRessourceViewModel(Release model)
        {
            Title = model.Title;
            Id = model.Id;
        }
    }
}
