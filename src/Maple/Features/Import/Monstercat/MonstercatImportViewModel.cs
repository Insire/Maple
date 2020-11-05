using FluentValidation.Results;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;
using System;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple
{
    public sealed class MonstercatImportViewModel : ViewModelListBase<IMonstercatViewModel>
    {
        private readonly ObservableCollection<ValidationFailure> _errors;
        private readonly ObservableCollection<MonstercatReleaseViewModel> _releases;
        private readonly ObservableCollection<MonstercatPlaylistViewModel> _playlists;

        private readonly IMonstercatViewModelFactory _monstercatApi;

        private string _source;
        public string Source
        {
            get { return _source; }
            set { SetValue(ref _source, value); }
        }

        private ValidationFailure _error;
        public ValidationFailure Error
        {
            get { return _error; }
            set { SetValue(ref _error, value); }
        }

        public ICommand ParseCommand { get; }

        public ICommand AddToResultsCommand { get; }

        public ICommand RemoveFromResultsCommand { get; }

        public ReadOnlyObservableCollection<ValidationFailure> Errors { get; }
        public ReadOnlyObservableCollection<MonstercatReleaseViewModel> Releases { get; }
        public ReadOnlyObservableCollection<MonstercatPlaylistViewModel> Playlists { get; }

        public MonstercatImportViewModel(in IScarletCommandBuilder commandBuilder, IMonstercatViewModelFactory monstercatApi)
            : base(commandBuilder)
        {
            _monstercatApi = monstercatApi ?? throw new ArgumentNullException(nameof(monstercatApi));

            _errors = new ObservableCollection<ValidationFailure>();
            _releases = new ObservableCollection<MonstercatReleaseViewModel>();
            _playlists = new ObservableCollection<MonstercatPlaylistViewModel>();

            Errors = new ReadOnlyObservableCollection<ValidationFailure>(_errors);
            Releases = new ReadOnlyObservableCollection<MonstercatReleaseViewModel>(_releases);
            Playlists = new ReadOnlyObservableCollection<MonstercatPlaylistViewModel>(_playlists);

            ParseCommand = commandBuilder
                .Create(Parse, CanParse)
                .WithSingleExecution()
                .Build();
        }

        private async Task Parse(CancellationToken token)
        {
            using (BusyStack.GetToken())
            {
                var result = await _monstercatApi.Create(Source, token);

                if (result is null)
                {
                    return;
                }

                await Add(result);
            }
        }

        private bool CanParse()
        {
            return !IsBusy && !string.IsNullOrWhiteSpace(Source);
        }
    }
}
