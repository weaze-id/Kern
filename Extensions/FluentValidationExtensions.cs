using System.Data.SqlTypes;
using FluentValidation;

namespace Kern.Extensions;

public static class FluentValidationExtensions
{
    public static IRuleBuilderOptions<T, TProperty> Required<T, TProperty>(this IRuleBuilder<T, TProperty> ruleBuilder)
    {
        return ruleBuilder
            .NotEmpty()
            .WithMessage("{PropertyName} is required.");
    }

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

    public static IRuleBuilderOptions<T, string?> CustomEmailAddress<T>(this IRuleBuilder<T, string?> ruleBuilder)
    {
        return ruleBuilder
            .EmailAddress()
            .WithMessage("{PropertyName} is not a valid email address.");
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

    public static IRuleBuilderOptions<T, string?> CustomMaxLength<T>(
        this IRuleBuilder<T, string?> ruleBuilder,
        int maxLength)
    {
        return ruleBuilder
            .MaximumLength(maxLength)
            .WithMessage($"Maximum characters is {maxLength}");
    }

    public static IRuleBuilderOptions<T, decimal> PrecissionScale<T>(
        this IRuleBuilder<T, decimal> ruleBuilder,
        int precission,
        int scale)
    {
        return ruleBuilder
            .Must(e =>
            {
                var sqlDecimal = new SqlDecimal(e);
                var scaleOverflow = 0;

                if (sqlDecimal.Scale > scale)
                {
                    scaleOverflow = sqlDecimal.Scale - scale;
                }

                return sqlDecimal.Precision - scaleOverflow <= precission;
            })
            .WithMessage($"{{PropertyName}} must be decimal with precision {precission} and scale {scale}");
    }

    public static IRuleBuilderOptions<T, decimal?> PrecissionScale<T>(
       this IRuleBuilder<T, decimal?> ruleBuilder,
       int precission,
       int scale)
    {
        return ruleBuilder
            .Must(e =>
            {
                var sqlDecimal = new SqlDecimal(e.GetValueOrDefault());
                var scaleOverflow = 0;

                if (sqlDecimal.Scale > scale)
                {
                    scaleOverflow = sqlDecimal.Scale - scale;
                }

                return sqlDecimal.Precision - scaleOverflow <= precission;
            })
            .WithMessage($"{{PropertyName}} must be decimal with precision {precission} and scale {scale}");
    }

    public static IRuleBuilderOptions<T, TProperty?> CustomGreaterThanOrEqualTo<T, TProperty>(
        this IRuleBuilder<T, TProperty?> ruleBuilder,
        TProperty valueToCompare) where TProperty : struct, IComparable<TProperty>, IComparable
    {
        return ruleBuilder
            .GreaterThanOrEqualTo(valueToCompare)
            .WithMessage("{PropertyName} must be greater than or equal to {ComparisonValue}");
    }
}