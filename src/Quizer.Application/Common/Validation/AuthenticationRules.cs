using FluentValidation;

namespace Quizer.Application.Common.Validation
{
    public static class AuthenticationRules
    {
        public static IRuleBuilderOptions<T, string> MustContainSubstring<T>(this IRuleBuilder<T, string> ruleBuilder, string substring)
        {
            return ruleBuilder
                .Must(property => property.Contains(substring))
                .WithErrorCode("ContainsSubstring")
                .WithMessage("The field must contain the specified substring.");
        }
    }
}
