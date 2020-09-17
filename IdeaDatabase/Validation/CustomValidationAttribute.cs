using System.ComponentModel.DataAnnotations;

namespace IdeaDatabase.Validation
{
    public class CustomValidationAttribute: ValidationAttribute
    {
        public ValidationResult IsValidExt(object value, ValidationContext validationContext)
        {
            return IsValid(value, validationContext);
        }
    }
}