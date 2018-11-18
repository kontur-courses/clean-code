using System.Collections.Generic;
using System.Linq;

namespace MarkDown
{
    public static class StringExtensions
    {
        public static string Escape(this string text, IEnumerable<string> specSymbols)
        {
            return specSymbols
                .Aggregate(text, (current, s) => current.Replace($@"\{s}", $"{s}"))
                .Replace(@"\\", @"\");
        }
    }
}
