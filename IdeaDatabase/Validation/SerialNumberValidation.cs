namespace IdeaDatabase.Validation
{
    public class SerialNumberValidation : StringValidation
    {
        public SerialNumberValidation(bool Required) :
            base(MinimumLength:1, MaximumLength:15, Required:Required, MatchRegexp: "^[a-zA-Z0-9]*$")
        {   
        }        
    }
}