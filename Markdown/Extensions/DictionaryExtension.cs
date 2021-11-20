using System;
using System.Collections.Generic;
using System.Linq;

namespace Markdown.Extensions
{
    internal static class DictionaryExtension
    {
        internal static Dictionary<int, TokenInfo> Validate(this Dictionary<int, TokenInfo> source)
        {
            return source
                .Where(x => x.Value.Valid)
                .ToDictionary(x => x.Key, x => x.Value);
        }

        internal static IEnumerable<Dictionary<TKey, TValue>> GroupToDictionaries<TKey, TValue, TGroup>(
            this Dictionary<TKey, TValue> source, 
            Func<KeyValuePair<TKey, TValue>, TGroup> groupFunc)
        {
            return source
                .GroupBy(groupFunc)
                .Select(x => x.ToDictionary(t => t.Key, t => t.Value));
        }
    }
}