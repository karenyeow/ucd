using System.Collections.Generic;
using System.Data;

namespace ICPFramework.DataAccess.DataReader
{
    public interface  IDataReaderMapper<out T>
    {
        T ToObject(IDataReader dataReader);
        IEnumerable<T> ToIEnumerable(IDataReader dataReader);
    }
}
