using System.Collections.Generic;

namespace Responses
{
    public class DuplicationValidationFault : ValidationFault
    {
        public ICollection<object> DuplicatedItems;

        public DuplicationValidationFault(string fName, string text, ICollection<object> duplicates) : base(fName, "DuplicatedItem", text)
        {
            DuplicatedItems = duplicates;
        }
    }
}
