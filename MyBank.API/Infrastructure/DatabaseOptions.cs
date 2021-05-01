using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
