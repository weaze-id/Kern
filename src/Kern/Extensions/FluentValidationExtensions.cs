using FluentValidation;

namespace Kern.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, string?> PhoneNumber<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Matches(@"^\+?\d{1,4}?\s?\(?\d{1,3}?\)?[-.\s]?\d{1,4}[-.\s]?\d{1,9}$")
            .WithMessage("{PropertyName} is not a valid phone number.");
    }

    public static IRuleBuilderOptions<T, string?> Url<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .Must(e => Uri.TryCreate(e, UriKind.Absolute, out _))
            .WithMessage("{PropertyName} is not a valid url");
    }
}