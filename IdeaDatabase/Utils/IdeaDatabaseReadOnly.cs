using IdeaDatabase.DataContext;

namespace IdeaDatabase.Utils
{
    public class IdeaDatabaseReadOnly : IdeaDatabaseDataContext
    {
        public IdeaDatabaseReadOnly():base(ConnectionConfig.DatabaseConnectionReadOnlyString)
        {
        }
    }
}