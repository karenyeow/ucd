
using System.Linq;
using Comlib.Common.Framework.DataAccess.Query;

namespace Comlib.Common.Framework.DataAccess.QueryHandler
{
    public class StoreProcQueryHandler : IQueryHandler
    {
        public void Assign(System.Data.SqlClient.SqlCommand command, IQuery query)
        {
            var spQuery = query as IStoredProcQuery;
            command.CommandType = System.Data.CommandType.StoredProcedure;
            command.CommandText = spQuery.StoredProcName;
            command.CommandTimeout = spQuery.QueryTimeOut;
            if (spQuery.Parameters != null && spQuery.Parameters.Count> 0)
            {
                command.Parameters.AddRange(spQuery.Parameters.ToArray());
            }

        }
    }
}
