namespace IdeaDatabase.Validation
{
    public class ProductNumberValidation : StringValidation
    {
        public ProductNumberValidation(bool Required, int MaximumLength = 50) :
            base(MinimumLength:0, MaximumLength: MaximumLength, Required:Required, MatchRegexp: @"^[a-zA-Z0-9_#\s]*$")
        {            
        }
    }
}