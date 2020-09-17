using ConstraintDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, IdeaDatabase.Validation.CustomValidationAttribute>>;
namespace IdeaDatabase.Validation
{
    public interface IDbFieldsConstraints
    {
        ConstraintDictionary Constraints { get; }
    }
}
