namespace Responses
{
    public abstract class ValidationFault : Fault
    {
        public string FieldName;
        public string ErrorType;

        public ValidationFault(string fName, string type, string status) :base ("InnovationPortal", "FieldValidationError", status)
        {
            ErrorType = type;
            FieldName = fName;
        }
    }
}
