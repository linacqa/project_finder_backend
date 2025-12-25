using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace Base.Helpers.Validation;

public class FileSizeAttribute(int minSize, int maxSize) : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is not IFormFile file) return ValidationResult.Success;
        if (file.Length < minSize) return new ValidationResult(GetTooSmallErrorMessage());
        if (file.Length > maxSize) return new ValidationResult(GetTooLargeErrorMessage());
        
        return ValidationResult.Success;
    }
    
    private string GetTooSmallErrorMessage() => $"Minimum allowed file size is {minSize} bytes.";
    private string GetTooLargeErrorMessage() => $"Maximum allowed file size is {maxSize} bytes.";
}