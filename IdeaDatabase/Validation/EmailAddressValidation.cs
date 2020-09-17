using Responses;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace IdeaDatabase.Validation
{
    public class EmailAddressValidation : CustomValidationAttribute
    {
        private int MaximumLength;
        private int MinimumLength;
        private bool Required;
        private bool Unicode;
        private string ForbiddenChars;

        public EmailAddressValidation(int MinimumLength = 0, int MaximumLength = int.MaxValue, bool Required = false, bool Unicode = true, string ForbiddenChars = null)
        {
            this.MaximumLength = MaximumLength;
            this.MinimumLength = MinimumLength;
            this.Required = Required;
            this.Unicode = Unicode;
            this.ForbiddenChars = ForbiddenChars;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                if (Required)
                    return new FaultValidationResult(new RequiredValidationFault(validationContext.MemberName));
                else
                    return ValidationResult.Success;
            }
            var ret = new FaultValidationResult();
            int size = value.ToString().Length;
            if (MinimumLength > size || size > MaximumLength)
                ret.fault.Add(new LengthValidationFault(validationContext?.MemberName, MinimumLength, MaximumLength));

            string email = value as string;
            if (ForbiddenChars != null && email.IndexOfAny(ForbiddenChars.ToCharArray()) != -1)
            {
                ret.fault.Add(new CharacterValidationFault(validationContext?.MemberName, ForbiddenChars));
            }

            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                
                string emailNormalized = email.ToString().Normalize(NormalizationForm.FormD);
                string addrNormalized = addr.Address.ToString().Normalize(NormalizationForm.FormD);

                if(!emailNormalized.Equals(addrNormalized))
                {
                    ret.fault.Add(new FormatValidationFault(validationContext?.MemberName, "xxx@xxx.xxx"));
                }                 
            }
            catch
            {
                ret.fault.Add(new FormatValidationFault(validationContext?.MemberName, "xxx@xxx.xxx"));
            }

            if (Unicode != true)
            {
                //ASCII max 127
                string s = value as string;
                if (s.Any(c => c > 127))
                    ret.fault.Add(new UnicodeValidationFault(validationContext?.MemberName));
            }

            return ret.fault.Count != 0 ? ret : ValidationResult.Success;
        }
    }
}