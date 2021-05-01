namespace MyBank.API.Infrastructure
{
    public class DatabaseOptions
    {

        public string ConnectionString { get; }
        public DatabaseOptions(string connectionString)
        {
            ConnectionString = connectionString;
        }


    }
}
