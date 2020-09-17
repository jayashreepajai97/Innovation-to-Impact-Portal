using IdeaDatabase.Enums;
using Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Validation
{
    public class OperatingSystemValidation : CustomValidationAttribute
    {
        private bool Required;
        private int MaximumLength;

        public OperatingSystemValidation(bool Required = false, int MaximumLength = 50)
        {
            this.MaximumLength = MaximumLength;
            this.Required = Required;            
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return Required ? new FaultValidationResult(new RequiredValidationFault(validationContext.MemberName)) : ValidationResult.Success;

            string sValue = value.ToString();
            
            // compare the length
            if (sValue.Length > this.MaximumLength)
                return new FaultValidationResult(new LengthValidationFault(validationContext.MemberName, 0, MaximumLength));

            // format should be "x.x.x.x"
            string[] words = sValue.Split('.');
            if( words.Count() != 4)
                return new FaultValidationResult(new FormatValidationFault(validationContext.MemberName, "N.N.N.N"));

            try
            {
                foreach (string w in words)
                    Int32.Parse(w);
            }
            catch (FormatException)
            {
                return new FaultValidationResult(new FormatValidationFault(validationContext.MemberName, "N.N.N.N"));
            }

            bool isWin10 = ((int)OS.Windows10 == Int32.Parse(words[0])) ? true : false;

            Type[] types = new Type[]
            {
                typeof(OS),
                typeof(OSEdition),
                typeof(OSArchitecture),
                typeof(OSServicePack)
            };

            int i = 0;

            FaultValidationResult faultResult = new FaultValidationResult();
            foreach (Type type in types)
            {
                if( type == typeof(OSServicePack) && isWin10 )
                {
                    // do not validate ServicePack for win10
                    continue;
                }
                if (Enum.GetName(type, Int32.Parse(words[i])) == null)
                {
                    faultResult.fault.Add(new EnumValidationFault(validationContext.MemberName + "." + type.Name, string.Join(", ", type.GetEnumNames().Zip((int[])type.GetEnumValues(), (x, y) => x + " - " + y))));
                }
                i++;
            }

            if (faultResult.fault.Count > 0)
                return faultResult;
            
            return ValidationResult.Success;
        }
    }
}