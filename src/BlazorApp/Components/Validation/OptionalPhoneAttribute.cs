using System.ComponentModel.DataAnnotations;

namespace Devpro.TodoList.BlazorApp.Components.Validation;

public class OptionalPhoneAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
        {
            return ValidationResult.Success;
        }

        var phoneAttribute = new PhoneAttribute
        {
            ErrorMessage = ErrorMessage ?? "The {0} field is not a valid phone number."
        };

        return phoneAttribute.IsValid(value)
            ? ValidationResult.Success
            : new ValidationResult(phoneAttribute.FormatErrorMessage(validationContext.DisplayName));
    }
}
