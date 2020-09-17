using Responses;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IdeaDatabase.Validation
{
    public class DuplicatesValidation : CustomValidationAttribute
    {
        private string FieldName;
        private bool ToUpper;
        private string DepedentFieldName;

        public DuplicatesValidation(string field, bool toupper = false, string depedentFieldName = null)
        {
            FieldName = field;
            ToUpper = toupper;
            DepedentFieldName = depedentFieldName;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            dynamic l = value;
            HashSet<object> duplicates = new HashSet<object>();
            HashSet<object> ids = new HashSet<object>();

            foreach (dynamic d in l)
            {
                if (d == null)
                    continue;

                if (string.IsNullOrEmpty(DepedentFieldName))
                    DuplicateCheck(d, duplicates, ids);
                else
                    DepedentDuplicateCheck(d, duplicates, ids);
            }

            if (duplicates.Count > 0)
                return new FaultValidationResult(new DuplicationValidationFault(FieldName, "Duplicated values for " + validationContext.MemberName, duplicates));

            return ValidationResult.Success;
        }

        private void DuplicateCheck(dynamic d, HashSet<object> duplicates, HashSet<object> ids)
        {
            var tmp = d.GetType().GetProperty(FieldName).GetValue(d, null);

            if (ToUpper)
                tmp = tmp?.ToUpper();

            if (!ids.Add(tmp))
                duplicates.Add(tmp);
        }

        private void DepedentDuplicateCheck(dynamic d, HashSet<object> duplicates, HashSet<object> ids)
        {
            var fName = d.GetType().GetProperty(FieldName).GetValue(d, null);
            var dFName = d.GetType().GetProperty(DepedentFieldName).GetValue(d, null);

            var tempField = dFName + fName;

            if (ToUpper)
                tempField = tempField?.ToUpper();

            if (!ids.Add(tempField))
                duplicates.Add(dFName + ":" + fName);
        }
    }
}