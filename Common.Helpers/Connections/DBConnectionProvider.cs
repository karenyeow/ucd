using Microsoft.Extensions.Configuration;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace Comlib.Common.Helpers.Connections
{
    abstract public class DBConnectionProvider : IDbConnectionProvider
    {
        private string _connectionStringName;
        private readonly IConfiguration _configuration;
        private DBEnum _db;
        public enum DBEnum { SQL, MYSQL}
        public DBConnectionProvider(string connectionStringName,  DBEnum db, IConfiguration configuration )
        {
            this._connectionStringName = connectionStringName;
            this._configuration = configuration;
            this._db = db;
        }
        public string ConnectionString
        {
            get
            {
                var connectionString = _configuration.GetConnectionString(this._connectionStringName);
                if (string.IsNullOrEmpty(connectionString))
                    throw new ArgumentNullException($"Cannot resolve " + this._connectionStringName + " connection string.");
                return connectionString;
            }
        }



        public  DbConnection CreateDBConnection()
        {
            switch (_db)
            {
                case DBEnum.MYSQL:
                    {
                        var sqlConnectionBuilder = new MySqlConnectionStringBuilder(this.ConnectionString);
                        return new MySqlConnection(sqlConnectionBuilder.ToString());
                    }
                case DBEnum.SQL:
          
                    {
                        return this.CreateSqlConnection();
                    }
                default:
                    {
                        throw new NotImplementedException();
                    }
           

            }
        }
        public  SqlConnection CreateSqlConnection()
        {
            if (this._db != DBEnum.SQL) throw new NotImplementedException();
            return new SqlConnection(this.ConnectionString);

        }
    }
}
