using System;
using System.Windows.Markup;

namespace InsireBot
{
    // Source: http://brianlagunas.com/a-better-way-to-data-bind-enums-in-wpf/
    public class EnumBindingSourceExtension : MarkupExtension
    {
        private Type _type;
        public Type Type
        {
            get { return _type; }
            set
            {
                if (value != _type)
                {
                    if (null != value)
                    {
                        var enumType = Nullable.GetUnderlyingType(value) ?? value;
                        if (!enumType.IsEnum)
                            throw new ArgumentException("Type must be for an Enum.");
                    }

                    _type = value;
                }
            }
        }

        public EnumBindingSourceExtension() { }

        public EnumBindingSourceExtension(Type type)
        {
            Type = type;
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            if (null == _type)
                throw new InvalidOperationException("The EnumType must be specified.");

            var actualEnumType = Nullable.GetUnderlyingType(_type) ?? _type;
            var enumValues = Enum.GetValues(actualEnumType);

            if (actualEnumType == _type)
                return enumValues;

            var tempArray = Array.CreateInstance(actualEnumType, enumValues.Length + 1);
            enumValues.CopyTo(tempArray, 1);
            return tempArray;
        }
    }
}
