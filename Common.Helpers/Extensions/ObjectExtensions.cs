using System;
using System.Collections.Generic;
using System.Text;

namespace Comlib.Common.Helpers.Extensions
{
    public static class ObjectExtensions
    {
        public static bool EqualsNullHandled(this object obj, object objCompare)
        {
            if (obj == null && objCompare == null)
                return true;
            if (obj == null && objCompare != null)
                return false;
            else if (obj != null && objCompare == null)
                return false;
            else
            {
                return obj.Equals(objCompare);
            }
        }
    }
}
