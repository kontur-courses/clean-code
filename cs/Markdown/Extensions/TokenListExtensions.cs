using System.Collections.Generic;
using System.Linq;
using Markdown.Parsers.Tokens;

namespace Markdown.Extensions
{
    public static class TokenListExtensions
    {
        public static void ToTextAt(this List<IToken> source, int idx)
        {
            source[idx] = source[idx].ToText();
        }

        public static void ToTextThatContainedIn(this List<IToken> source, List<IToken> collection)
        {
            var tokenIndexes = collection.Select(token => source.FindIndex(el => el == token));
            foreach (var idx in tokenIndexes)
                source.ToTextAt(idx);
        }
    }
}
