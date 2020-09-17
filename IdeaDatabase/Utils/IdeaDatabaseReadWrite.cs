using System;
using IdeaDatabase.DataContext;

namespace IdeaDatabase.Utils
{
    public class IdeaDatabaseReadWrite : IdeaDatabaseDataContext
    {
        public IdeaDatabaseReadWrite() : base(ConnectionConfig.DatabaseConnectionReadWriteString)
            {
        }
    }
}