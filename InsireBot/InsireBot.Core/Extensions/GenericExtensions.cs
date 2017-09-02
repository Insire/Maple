using System;
using System.Runtime.CompilerServices;
using Maple.Localization.Properties;

namespace Maple.Core.Extensions
{
    public static class GenericExtensions
    {
        public static T ThrowIfNull<T>(this T obj, string objName, [CallerMemberName]string callerName = null)
        {
            if (obj == null)
                throw new ArgumentNullException(objName, string.Format("{0} {1} (2)", objName, Resources.IsRequired, callerName));

            return obj;
        }
    }
}
