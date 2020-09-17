using Hpcs.DependencyInjector;

using Responses;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace IdeaDatabase.Validation
{
    public class DbFieldValidation : ValidationAttribute
    {
        private CustomValidationAttribute attribute;
        private IDbFieldsConstraints dbField = DependencyInjector.Get<IDbFieldsConstraints, DbFieldsConstraints>();

        public DbFieldValidation(string DbClassName, [CallerMemberName] string fieldName = null)
        {           
            attribute = dbField.Constraints[DbClassName][fieldName];            
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (attribute == null)
            {
                return new FaultValidationResult(new NullValidatorFault(validationContext.MemberName));
            }
            return attribute.IsValidExt(value, validationContext);
        }
   }
}