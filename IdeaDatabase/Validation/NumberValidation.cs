using Responses;
using System;
using System.ComponentModel.DataAnnotations;

namespace IdeaDatabase.Validation
{
    public class NumberValidation : CustomValidationAttribute
    {
        private object min;
        private object max;
        private bool required;
        private bool fromString;
        public NumberValidation(object min, object max = null, bool required = false, bool fromString = false)
        {
            this.min = min;
            this.max = max;
            this.required = required;
            this.fromString = fromString;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                if (required)
                {
                    return new FaultValidationResult(new RequiredValidationFault(validationContext.MemberName));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }

            if( fromString )
            {
                // test if it is a number
                int number = 0;
                if (Int32.TryParse(value.ToString(), out number) == false)
                {
                    return new FaultValidationResult(new NumberValidationFault(validationContext.MemberName, min, max));
                }
                value = number;
            }

            var tmp = (IComparable)value;
            if (tmp.CompareTo(min) == -1 || (max != null && tmp.CompareTo(max) == 1))
            {
                return new FaultValidationResult(new NumberValidationFault(validationContext.MemberName, min, max));
            }

            return ValidationResult.Success;
        }

    }
}
