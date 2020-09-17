using Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IdeaDatabase.Validation
{
    public class EnumValidation : CustomValidationAttribute
    {
        public Type typeOfEnum;
        private bool required;
        private bool byValue;
        private bool byDescription;
        public EnumValidation(Type t, bool required = false, bool byValue = false, bool byDescription = false)
        {
            typeOfEnum = t;
            this.required = required;   
            this.byValue = byValue;
            this.byDescription = byDescription;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return required ? new FaultValidationResult(new RequiredValidationFault(validationContext.MemberName)) : ValidationResult.Success;
            }
            if (byDescription)
            {
                if (string.IsNullOrEmpty(value.ToString()) && !required)
                {
                    return ValidationResult.Success;
                }
                List<string> allowed = new List<string>();
                Array values = Enum.GetNames(typeOfEnum);
                foreach (string val in values)
                {                    
                    var memInfo = typeOfEnum.GetMember(val);
                    var descriptionAttributes = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                    if (descriptionAttributes.Length > 0)
                    {
                        string description = ((DescriptionAttribute)descriptionAttributes[0]).Description;
                        if (description.Equals(value.ToString()))
                        {
                            return ValidationResult.Success;
                        }
                        allowed.Add(description);
                    }
                }

                return new FaultValidationResult(new EnumValidationFault(validationContext.MemberName, String.Join(", ", allowed)));
            }

            if (byValue)
            {
                if ((!Enum.IsDefined(typeOfEnum, value)))
                {
                    return new FaultValidationResult(new EnumValidationFault(validationContext.MemberName, string.Join(", ", typeOfEnum.GetEnumNames().Zip((int[])typeOfEnum.GetEnumValues(), (x, y) => x + " - " + y))));
                }
                else
                {
                    return ValidationResult.Success;
                }
            }
            else if (!Enum.GetNames(typeOfEnum).Contains(value.ToString()))
            {
                return new FaultValidationResult(new EnumValidationFault(validationContext.MemberName, String.Join(", ", Enum.GetNames(typeOfEnum))));
            }

            return ValidationResult.Success;
        }        
    }
}