using System.Data.Common;

namespace Comlib.Common.Helpers.Connections
{
    public interface IDbConnectionProvider
    {
        DbConnection Create();
    }
}
