namespace Responses
{
    public class RequiredValidationFault : ValidationFault
    {
        public RequiredValidationFault(string field) : base(field, "Required", "Required field is missing")
        {

        }
    }
}
