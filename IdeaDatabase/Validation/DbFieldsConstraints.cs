using IdeaDatabase.DataContext;
using IdeaDatabase.Utils;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

using ConstraintDictionary = System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<string, IdeaDatabase.Validation.CustomValidationAttribute>>;
namespace IdeaDatabase.Validation
{
    public class DbFieldsConstraints : IDbFieldsConstraints
    {
        public ConstraintDictionary Constraints { get { return _constraints; } }

        private static ConstraintDictionary _constraints = BuildConstraints();        
        private static ConstraintDictionary BuildConstraints()
        {
            ConstraintDictionary d = new ConstraintDictionary();

            using (IdeaDatabaseDataContext context = new IdeaDatabaseReadOnly())
            {
                var objectContext = ((IObjectContextAdapter)context).ObjectContext;
                var container = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
                foreach (var table in container.BaseEntitySets.Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EntitySet))
                {
                    Dictionary<string, CustomValidationAttribute> tmp = new Dictionary<string, CustomValidationAttribute>();
                    foreach (EdmProperty column in table.ElementType.Members.Where(x => x.BuiltInTypeKind == BuiltInTypeKind.EdmProperty))
                    {
                        switch (column.PrimitiveType.PrimitiveTypeKind)
                        {                          

                            case PrimitiveTypeKind.String:
                                {
                                    int maxLength = int.MaxValue;
                                    if (column.MaxLength != null)
                                    {
                                        maxLength = column.MaxLength.Value;
                                    }
                                    tmp.Add(column.Name, new StringValidation(MaximumLength: maxLength, Required: !column.Nullable, MinimumLength: column.Nullable ? 0 : 1));

                                }
                                break;
                            case PrimitiveTypeKind.DateTime:
                                tmp.Add(column.Name, new DateTimeValidation(required: !column.Nullable));
                                break;
                        }
                        
                    }
                    if (tmp.Count > 0)
                        d.Add(table.Name, tmp);
                }                
            }

            return d;
        }
    }
}