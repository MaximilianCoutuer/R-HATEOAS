using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Extensions
{
    public static class TypeListExtMethod
    {
        public static bool IsList(this Type type)
        {
            return (type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(List<>)));
        }

    }
}
