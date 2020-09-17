namespace IdeaDatabase.Validation
{
    public class VersionStringValidation : StringValidation
    {
        public VersionStringValidation(bool Required) :
            base(MinimumLength: 0, MaximumLength: 15, Required: Required, MatchRegexp: "^(\\d+\\.){1,}(\\d+)$")
        {
        }
    }
}