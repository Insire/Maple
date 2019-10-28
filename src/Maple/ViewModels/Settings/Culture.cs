using System.Globalization;
using MvvmScarletToolkit;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public sealed class Culture : ViewModelBase<CultureInfo>
    {
        public string DisplayName => Model.DisplayName;

        public Culture(ICommandBuilder commandBuilder, CultureInfo culture)
            : base(commandBuilder, culture)
        {
        }
    }
}
