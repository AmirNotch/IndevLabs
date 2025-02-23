using System.ComponentModel.DataAnnotations;

namespace IndevLabs.Validation.Attributes;

public class ValidIntAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
    {
        // Проверяем, что значение не null и является типом int
        if (value == null || value is not int intValue)
        {
            return new ValidationResult("Invalid integer value.");
        }

        // Проверяем, что значение не равно нулю (или другому недопустимому значению)
        if (intValue <= 0) // Например, отрицательные числа или ноль недопустимы
        {
            return new ValidationResult("Value must be a positive integer.");
        }

        // Если всё в порядке, возвращаем успешный результат
        return ValidationResult.Success!;
    }
}