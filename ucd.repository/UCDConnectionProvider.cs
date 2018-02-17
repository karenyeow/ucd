using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace UCD.Repository
{
   public class UCDConnectionProvider:IUCDConnectionProvider 
    {
        private readonly IConfiguration _configuration;
        public UCDConnectionProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string ConnectionString {
            get
            {
                var connectionString = _configuration.GetConnectionString("UCD");
                if (string.IsNullOrEmpty(connectionString))
                {
                    throw new ArgumentNullException($"Cannot resolve UCD connection string.");
                }
                return connectionString;
            } }

        public DbConnection Create()
        {
            return new SqlConnection(this.ConnectionString);
        }
    }
}
