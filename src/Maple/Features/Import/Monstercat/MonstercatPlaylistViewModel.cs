namespace Maple
{
    public sealed class MonstercatPlaylistViewModel : MonstercatRessourceViewModel
    {
        public int NumRecords { get; }
        public bool Public { get; }
        public bool MyLibrary { get; }

        public MonstercatPlaylistViewModel(SoftThorn.MonstercatNet.Playlist model)
            : base(model)
        {
            NumRecords = model.NumRecords;
            Public = model.Public;
            MyLibrary = model.MyLibrary;
        }
    }
}
