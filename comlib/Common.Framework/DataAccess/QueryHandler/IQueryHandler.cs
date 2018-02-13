using System.Data.SqlClient;
using Comlib.Common.Framework.DataAccess.Query;

namespace Comlib.Common.Framework.DataAccess.QueryHandler
{
   public interface IQueryHandler
    {
       void Assign (SqlCommand command, IQuery query);
    }
}
