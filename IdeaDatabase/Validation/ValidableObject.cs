using Responses;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace IdeaDatabase.Validation
{
    public abstract class ValidableObject
    {
        public bool IsValid(ResponseBase r)
        {
            var results = new List<ValidationResult>();
            if (this != null)
            {
                this.Validate(results, r);
            }
            return r.ErrorList.Count == 0;
        }

        protected virtual void Validate(List<ValidationResult> results, ResponseBase r)
        {
            var context = new ValidationContext(this, null, null);
            if (!Validator.TryValidateObject(this, context, results, true))
            {
                r.ErrorList.UnionWith(results.OfType<FaultValidationResult>().SelectMany(t => t.fault));
            }

            foreach (var content in this.GetType().GetProperties())
            {
                if (content.PropertyType.Name.Equals("List`1"))
                {
                    var ll = content.GetValue(this, null);
                    if ((IList)ll != null)
                    {
                        foreach (var l in (IList)ll)
                        {
                            if (l != null)
                            {
                                dynamic dObj = l;
                                dObj.IsValid(r);
                            }
                        }
                    }
                }
                else if (content.PropertyType.IsSubclassOf(typeof(ValidableObject)))
                {
                    ValidableObject obj = content.GetValue(this, null) as ValidableObject;
                    if (obj != null)
                    {
                        obj.Validate(results, r);
                    }
                }
            }
        }
    }
}