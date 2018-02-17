using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Comlib.Common.Helpers.Connections
{
    public class ApplicationConnectionProvider : IApplicationConnectionProvider
    {
        private readonly IConfiguration _configuration;
        public ApplicationConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString
        {
            get
            {
                var connectionString = _configuration.GetConnectionString("Application");
                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException($"Cannot resolve Application connection string.");
                return connectionString;
            }
        }

        public DbConnection Create()
        {
            

            var sqlConnectionBuilder = new SqlConnectionStringBuilder(this.ConnectionString);
            return new SqlConnection(sqlConnectionBuilder.ToString());
        }

        public SqlConnection CreateSqlConnection()
        {
            throw new NotImplementedException();
        }
    }
}