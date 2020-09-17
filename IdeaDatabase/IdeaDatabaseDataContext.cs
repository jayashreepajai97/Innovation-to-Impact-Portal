namespace IdeaDatabase.DataContext
{
    public partial class IdeaDatabaseDataContext
    {        
        public IdeaDatabaseDataContext(string connectionString) : base(connectionString)
        {
            //this.Transaction = this.Connection.BeginTransaction();
            //OnCreated();
        }
    }
}