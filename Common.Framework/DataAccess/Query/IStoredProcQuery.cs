using System.Collections.Generic;
using System.Data.SqlClient;

namespace Comlib.Common.Framework.DataAccess.Query
{
    public interface IStoredProcQuery : IQuery
    {
        string StoredProcName { get; }
        IList<SqlParameter> Parameters { get; }

    }
}
