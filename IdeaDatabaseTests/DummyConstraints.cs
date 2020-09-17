using IdeaDatabase.Validation;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

using ConstraintDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, IdeaDatabase.Validation.CustomValidationAttribute>>;
namespace DeviceDatabaseTests
{
    [ExcludeFromCodeCoverage]
    public class DummyConstraints
    {
        private ConstraintDictionary _constraints;

        public DummyConstraints()
        {
            _constraints = new ConstraintDictionary();
        }

        public ConstraintDictionary GetConstraints()
        {
            return _constraints;
        }

        public void StringSingleConstraints(string tableName, string fieldName, int maxlength, bool required, int minLength)
        {            
            Dictionary<string, CustomValidationAttribute> tmp = new Dictionary<string, CustomValidationAttribute>();
            tmp.Add(fieldName, new StringValidation(MaximumLength: maxlength, Required: required, MinimumLength: minLength));

            _constraints.Add(tableName, tmp);
        }

        public void DateTimeSingleConstraints(string tableName, string fieldName, bool required)
        {
            Dictionary<string, CustomValidationAttribute> tmp = new Dictionary<string, CustomValidationAttribute>();
            tmp.Add(fieldName, new DateTimeValidation(required: required));

            _constraints.Add(tableName, tmp);
        }
     
    }
}
