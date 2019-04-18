using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Extensions
{
    public static class DictionaryExt
    {
        public static U GetValueForKeyOrDefault<T, U> (this IDictionary<T, U> dictionary, T key)
        {
            return dictionary.TryGetValue(key, out U value) ? value : default(U);
        }
    }
}
