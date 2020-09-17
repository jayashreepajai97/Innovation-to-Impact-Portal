using Responses;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.ModelBinding;

namespace IdeaDatabase.Validation
{
    public class RequiredValidation : CustomValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new FaultValidationResult(new RequiredValidationFault(validationContext.MemberName));
            }
            return ValidationResult.Success;
        }
    }
}