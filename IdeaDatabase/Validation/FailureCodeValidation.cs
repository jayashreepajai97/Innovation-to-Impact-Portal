using Responses;
using System.ComponentModel.DataAnnotations;

namespace IdeaDatabase.Validation
{
    public enum FailureCodeValidationEnum
    {
        Category = 0
    }
    public class FailureCodeValidation : CustomValidationAttribute
    {
        private bool Required;
        private int MaximumLength;
        private int MinimumLength;
        private string DependentFieldName;

        public FailureCodeValidation(bool Required = false, int MinimumLength = 0, int MaximumLength = int.MaxValue, FailureCodeValidationEnum DependentFieldName = FailureCodeValidationEnum.Category)
        {
            this.Required = Required;
            this.MaximumLength = MaximumLength;
            this.MinimumLength = MinimumLength;
            this.DependentFieldName = DependentFieldName.ToString();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                if (Required)
                {
                    return new FaultValidationResult(new RequiredValidationFault(validationContext.MemberName));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            var ret = new FaultValidationResult();
            int size = value.ToString().Length;
            if (MinimumLength > size || size > MaximumLength)
            {
                ret.fault.Add(new LengthValidationFault(validationContext.MemberName, MinimumLength, MaximumLength));
            }

            string s = value.ToString();
            var otherValue = validationContext.ObjectType.GetProperty(DependentFieldName).GetValue(validationContext.ObjectInstance, null);

            

            return ret.fault.Count != 0 ? ret : ValidationResult.Success;
        }
    }
}