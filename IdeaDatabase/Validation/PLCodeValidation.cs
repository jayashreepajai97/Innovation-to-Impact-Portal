using Responses;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace IdeaDatabase.Validation
{
    public class PLCodeValidation: StringValidation 
    {
        private const string HpProductFieldName = "IsHPProduct";
        private readonly int maxNonHpPlCodeLength;
        private readonly int maxHpPlCodeLength;

        public PLCodeValidation(int minimumPlCodeLength, int maxHpPlCodeLength, int maxNonHpPlCodeLength, bool required = false) 
            : base(minimumPlCodeLength, maxHpPlCodeLength, required)
        {
            this.maxNonHpPlCodeLength = maxNonHpPlCodeLength;
            this.maxHpPlCodeLength = maxHpPlCodeLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var ret = new FaultValidationResult();
            int size = value != null? value.ToString().Length : 0;
            bool isHpProduct = true;

            PropertyInfo propertyName = validationContext.ObjectType.GetProperty(HpProductFieldName);
            if (propertyName != null)
            {
                isHpProduct = (bool)propertyName.GetValue(validationContext.ObjectInstance, null);
                MaximumLength = maxHpPlCodeLength;
            }

            if (!isHpProduct)
            {
                MaximumLength = maxNonHpPlCodeLength;
            }

            return base.IsValid(value, validationContext);
        }
    }
}