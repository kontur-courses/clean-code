using System.Collections.Generic;
using System.Linq;
using Markdown.Tokens;

namespace Markdown.Extensions
{
    public static class CharExtensions
    {
        private static readonly HashSet<char> charset = GetTags().ToHashSet();

        private static IEnumerable<char> GetTags()
        {
            yield return new ItalicToken(0).Value[0];
            yield return new HeadingToken(0, true).Value[0];
            yield return new EscapeToken(0).Value[0];
        }

        public static bool IsTagSymbol(this char s)
            => charset.Contains(s);
    }
}
