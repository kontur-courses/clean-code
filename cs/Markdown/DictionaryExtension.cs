using System.Collections.Generic;
using System.Linq;
namespace Markdown
{
    public static class DictionaryExtension
    {
        public static T TryGetKeyByValue<T>(this Dictionary<T, T> dict, T value)
        {
            return dict.FirstOrDefault(x => x.Value.Equals(value)).Key;
        }
    }
}