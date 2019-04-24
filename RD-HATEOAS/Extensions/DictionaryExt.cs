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
        /// This saves on error handling.
        /// </summary>
        /// <typeparam name="K">Key</typeparam>
        /// <typeparam name="V">Value</typeparam>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">Key to search for.</param>
        /// <returns></returns>
        public static V GetValueForKeyOrDefault<K, V> (this IDictionary<K, V> dictionary, K key)
        {
            return dictionary.TryGetValue(key, out V value) ? value : default(V);
        }
    }
}
