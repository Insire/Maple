using System.Globalization;
using MvvmScarletToolkit.Abstractions;
using MvvmScarletToolkit.Observables;

namespace Maple
{
    public class Culture : ViewModelBase<CultureInfo>
    {
        public string DisplayName => Model.DisplayName;

        public Culture(CultureInfo info, ICommandBuilder commandBuilder)
            : base(commandBuilder, info)
        {
        }
    }
}
