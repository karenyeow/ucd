using System.Data.Common;

namespace Common.Helpers.Connections
{
    public interface IDbConnectionProvider
    {
        DbConnection Create();
    }
}
