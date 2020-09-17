using System;
using Responses;
using System.Linq;
using IdeaDatabase.Enums;
using System.ComponentModel.DataAnnotations;
 

namespace IdeaDatabase.Validation
{
    public class EventCategoryValidation : CustomValidationAttribute
    {
        private bool Required;
        private int MaximumLength;
        private int MinimumLength;

        public EventCategoryValidation(bool Required = false, int MinimumLength = 0, int MaximumLength = int.MaxValue)
        {
            this.Required = Required;
            this.MaximumLength = MaximumLength;
            this.MinimumLength = MinimumLength;
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

 

            return ret.fault.Count != 0 ? ret : ValidationResult.Success;
        }
    }
}