using Comlib.Common.Helpers.Utils;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Comlib.Common.Framework.DataAccess.Query
{
    public class StoredProcQuery : IStoredProcQuery
    {
        public StoredProcQuery(string procName, object parameters)
        {
            StoredProcName = procName;
            Parameters = new List<SqlParameter>();
            foreach (var param in DictionaryUtils.AnonToDictionary(parameters))
            {
                Parameters.Add(new SqlParameter(param.Key, param.Value ?? DBNull.Value));   
            }
        }
        
        public string StoredProcName { get; private set; }

        public IList<System.Data.SqlClient.SqlParameter> Parameters { get; private set; }

        public string Text
        {
            get { throw new NotImplementedException(); }
        }

        public int QueryTimeOut
        {
            get { return 300; }
        }
    }
}
