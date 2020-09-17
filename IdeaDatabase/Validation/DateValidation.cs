using IdeaDatabase.Utils;
using Responses;
using System.ComponentModel.DataAnnotations;

namespace IdeaDatabase.Validation
{
    public class DateTimeValidation : CustomValidationAttribute
    {
        private bool required;
        private string format;

        public DateTimeValidation(bool required = false, string format = CustomDateTimeFormat.AllowedFormats)
        {
            this.required = required;
            this.format = format;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            CustomDateTime d = new CustomDateTimeFormat((string)value, format);

            if (d.Text == null && required)
            {
                return new FaultValidationResult(new RequiredValidationFault(validationContext.MemberName));
            }

            if (d.Text != null && d.IsValid == false)
            {
                return new FaultValidationResult(new FormatValidationFault(validationContext.MemberName, format));
            }

            return ValidationResult.Success;
        }

    }
}