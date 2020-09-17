namespace Responses
{
    public class NullValidatorFault : ValidationFault
    {
        public string fieldName;

        public NullValidatorFault(string fieldName) : base(fieldName, "Validator", "No matching validator found for this field")
        {
            this.fieldName = fieldName;
        }
    }
}
