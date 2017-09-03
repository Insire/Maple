using FluentValidation;
using FluentValidation.Internal;
using FluentValidation.Results;

namespace Maple.Core
{
    public static class IValidatorExtensions
    {
        public static ValidationResult Validate<T>(this IValidator validator, T instance, params string[] properties)
        {
            var context = new ValidationContext<T>(instance, new PropertyChain(), ValidatorOptions.ValidatorSelectors.MemberNameValidatorSelectorFactory(properties));
            return validator.Validate(context);
        }
    }
}
