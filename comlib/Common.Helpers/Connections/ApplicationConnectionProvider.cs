using System;
using System.Data.Common;
using System.Data.SqlClient;
using Microsoft.Extensions.Configuration;

namespace Common.Helpers.Connections
{
    public class ApplicationConnectionProvider : IApplicationConnectionProvider
    {
        private readonly IConfiguration _configuration;
        public ApplicationConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public DbConnection Create()
        {
            var connectionString = _configuration.GetConnectionString("Application");
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException($"Cannot resolve Application connection string.");

            var sqlConnectionBuilder = new SqlConnectionStringBuilder(connectionString);
            return new SqlConnection(sqlConnectionBuilder.ToString());
        }

        public SqlConnection CreateSqlConnection()
        {
            throw new NotImplementedException();
        }
    }
}