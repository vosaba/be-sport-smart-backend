using System.ComponentModel.DataAnnotations;

namespace Bss.Infrastructure.Shared.Attributes;

public class EnumSubsetAttribute : ValidationAttribute
{
    private readonly HashSet<Enum> _validValues;

    public EnumSubsetAttribute(params object[] validValues)
    {
        if (validValues == null || !validValues.All(v => v.GetType().IsEnum))
        {
            throw new ArgumentException("Values must be enum values.");
        }

        _validValues = new HashSet<Enum>(validValues.Cast<Enum>());
    }

    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null)
        {
            return new ValidationResult($"The value of {validationContext.DisplayName} is required.");
        }

        if (!value.GetType().IsEnum)
        {
            throw new ArgumentException("This attribute can only be used with enumerations.");
        }

        if (!_validValues.Contains((Enum)value))
        {
            return new ValidationResult($"The value of {validationContext.DisplayName} is not valid.");
        }

        return ValidationResult.Success!;
    }
}
