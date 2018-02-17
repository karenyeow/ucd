using System.Data.Common;
using System.Data.SqlClient;

namespace Comlib.Common.Helpers.Connections
{
    public interface IDbConnectionProvider
    {
        DbConnection CreateDBConnection();
        SqlConnection CreateSqlConnection();
        string ConnectionString { get; }

    }
}
