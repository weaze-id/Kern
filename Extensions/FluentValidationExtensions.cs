using FluentValidation;

namespace Kern.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> Password<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(e => (e?.Length ?? 0) >= 8)
            .WithMessage("{PropertyName} must be at least 8 characters.");
    }

    public static IRuleBuilderOptions<T, string?> PhoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Matches("^08[0-9]{9,10}$")
            .WithMessage("{PropertyName} is not a valid phone number.");
    }

    public static IRuleBuilderOptions<T, string?> Url<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(e =>
            {
                if (string.IsNullOrWhiteSpace(e))
                {
                    return true;
                }

                return Uri.TryCreate(e, UriKind.Absolute, out _);
            })
            .WithMessage("{PropertyName} is not a valid url");
    }
}