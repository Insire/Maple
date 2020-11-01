using FluentValidation.Results;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;
using SoftThorn.MonstercatNet;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;

namespace Maple
{
    public sealed class MonstercatImportViewModel : ViewModelListBase<MonstercatRessourceViewModel>
    {
        private readonly ObservableCollection<ValidationFailure> _errors;
        private readonly IMonstercatApi _monstercatApi;

        private string _source;
        public string Source
        {
            get { return _source; }
            set { SetValue(ref _source, value); }
        }

        public ReadOnlyObservableCollection<ValidationFailure> Errors { get; }

        private ValidationFailure _error;
        public ValidationFailure Error
        {
            get { return _error; }
            set { SetValue(ref _error, value); }
        }

        public ICommand ParseCommand { get; }

        public MonstercatImportViewModel(in IScarletCommandBuilder commandBuilder, IMonstercatApi monstercatApi)
            : base(commandBuilder)
        {
            _monstercatApi = monstercatApi ?? throw new System.ArgumentNullException(nameof(monstercatApi));

            ParseCommand = commandBuilder
                .Create(Parse, CanParse)
                .WithSingleExecution()
                .Build();
        }

        private async Task Parse(CancellationToken token)
        {
            using (BusyStack.GetToken())
            {
                var result = await _monstercatApi
                    .Parse(Source, token);

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
