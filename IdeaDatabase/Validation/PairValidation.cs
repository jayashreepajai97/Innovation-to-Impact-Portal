using Responses;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace IdeaDatabase.Validation
{
    public class PairValidation : CustomValidationAttribute
    {
        private string FirstFieldName;
        private string SecondFieldName;
        private bool ToUpper;

        public PairValidation(string firstField, string secondField, bool toupper = false)
        {
            FirstFieldName = firstField;
            SecondFieldName = secondField;
            ToUpper = toupper;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            dynamic l = value;

            HashSet<Tuple<object, object>> duplicates = new HashSet<Tuple<object, object>>();
            HashSet<Tuple<object, object>> ids = new HashSet<Tuple<object, object>>();
            foreach (dynamic d in l)
            {
                if( d == null )
                    continue;
                
                object tmp1 = d.GetType().GetProperty(FirstFieldName).GetValue(d, null);
                object tmp2 = d.GetType().GetProperty(SecondFieldName).GetValue(d, null);
                                
                if (!ids.Add(Tuple.Create(tmp1, tmp2)))                
                    duplicates.Add(Tuple.Create(tmp1, tmp2));
            }
            if (duplicates.Count > 0)
                return new FaultValidationResult(new DuplicationValidationFault(FirstFieldName+ "-" + SecondFieldName, "Duplicated values for pair " + FirstFieldName + "-" + SecondFieldName, null));

            return ValidationResult.Success;

        }
    }
}