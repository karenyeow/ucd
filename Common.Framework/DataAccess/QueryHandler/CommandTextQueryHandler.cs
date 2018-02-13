

using Comlib.Common.Framework.DataAccess.Query;

namespace Comlib.Common.Framework.DataAccess.QueryHandler
{
  public class CommandTextQueryHandler : IQueryHandler
    {
        public void Assign(System.Data.SqlClient.SqlCommand command, IQuery query)
        {
            command.CommandType = System.Data.CommandType.Text;
            command.CommandText = query.Text;
            command.CommandTimeout = query.QueryTimeOut;
        }
    }
}
