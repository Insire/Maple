using System.Globalization;
using Maple.Core;

namespace Maple
{
    public class Culture : BaseViewModel<CultureInfo>
    {
        public string DisplayName => Model.DisplayName;

        public Culture(CultureInfo info, IMessenger messenger) : base(info, messenger)
        {

        }
    }
}
