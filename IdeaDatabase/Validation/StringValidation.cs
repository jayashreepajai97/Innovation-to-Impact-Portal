using System.ComponentModel.DataAnnotations;
using Responses;
using System.Text.RegularExpressions;
using System.Linq;

namespace IdeaDatabase.Validation
{
    public class StringValidation : CustomValidationAttribute
    {
        protected int MaximumLength; 
        protected int MinimumLength;
        private bool Required;
        private string ForbiddenChars;
        private string MatchRegexp;
        private bool Unicode;
        private bool PasswordCheck;

        public StringValidation(int MinimumLength = 0, int MaximumLength = int.MaxValue, bool Required = false,
            string ForbiddenChars = null, string MatchRegexp = null, bool Unicode = true, bool PasswordCheck = false)
        {
            this.MaximumLength = MaximumLength;
            this.MinimumLength = MinimumLength;
            this.Required = Required;
            this.ForbiddenChars = ForbiddenChars;
            this.MatchRegexp = MatchRegexp;
            this.Unicode = Unicode;
            this.PasswordCheck = PasswordCheck;
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
            if (ForbiddenChars != null && s.IndexOfAny(ForbiddenChars.ToCharArray()) != -1)
            {
                ret.fault.Add(new CharacterValidationFault(validationContext.MemberName, ForbiddenChars));
            }
            if (MatchRegexp != null)
            {
                Regex re = new Regex(MatchRegexp);
                if (!re.Match(s).Success)
                {
                    if (PasswordCheck == true)
                    {
                        // The password must contain three of the following:  
                        //      English uppercase characters (A through Z)
                        //      English lowercase characters (a through z)
                        //      numerals (0 through 9)
                        //      non-alphabetic characters (such as !, $, #, %)
                        ret.fault.Add(new FormatValidationFault(validationContext.MemberName, "Password must be at least 8 characters and contain characters from at least 3 of the following groups: uppercase, lowercase, numerals, or symbols."));
                    }
                    else
                    {
                        ret.fault.Add(new FormatValidationFault(validationContext.MemberName, MatchRegexp));
                    }
                }
            }
            if (Unicode != true)
            {
                //ASCII max 127
                if (s.Any(c => c > 127))
                {
                    ret.fault.Add(new UnicodeValidationFault(validationContext.MemberName));
                }
            }
            return ret.fault.Count != 0 ? ret : ValidationResult.Success;
        }
    }
}