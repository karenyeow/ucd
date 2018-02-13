using System;
using System.Collections.Generic;
using System.Text;

namespace Comlib.Common.Framework.DataAccess.Query
{
    public interface IQuery
    {
        string Text { get; }

        int QueryTimeOut { get; }
    }
}
