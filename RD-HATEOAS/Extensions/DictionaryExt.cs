using System;
using System.Collections.Generic;
using System.Text;

namespace RDHATEOAS.Extensions
{
    public static class DictionaryExt
    {
        /// <summary>
        /// Returns the value for a given key in a dictionary.
        /// Returns default (instead of an exception) if the key isn't in the dictionary.
        /// </summary>
        /// <typeparam name="T">Key</typeparam>
        /// <typeparam name="U">Value</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">Key to search for.</param>
        /// <returns></returns>
        public static U GetValueForKeyOrDefault<T, U> (this IDictionary<T, U> dictionary, T key)
        {
            return dictionary.TryGetValue(key, out U value) ? value : default(U);
        }
    }
}
