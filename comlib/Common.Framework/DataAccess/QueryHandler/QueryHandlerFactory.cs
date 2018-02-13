using Comlib.Common.Framework.DataAccess.Query;
using System;

namespace Comlib.Common.Framework.DataAccess.QueryHandler
{
    public static class QueryHandlerFactory
    {
        public static IQueryHandler Create(IQuery query)
        {
            if (query is ICommandTextQuery)
            {
                return new CommandTextQueryHandler();
            }

            if (query is IStoredProcQuery)
            {
                return new StoreProcQueryHandler();
            }

            var ex = new NotSupportedException();
            ex.Data.Add("IQuery Type", query.GetType());
            throw ex;
        }
    }
}
