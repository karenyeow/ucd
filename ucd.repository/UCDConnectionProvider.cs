using Comlib.Common.Helpers.Connections;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;

namespace UCD.Repository
{
   public class UCDConnectionProvider: DBConnectionProvider  , IUCDConnectionProvider
    {
        public UCDConnectionProvider(IConfiguration configuration):base("UCD",DBEnum.SQL , configuration)
        {  }
  

    }
}
