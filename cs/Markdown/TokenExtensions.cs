using System.Collections.Generic;

namespace Markdown
{
    static class TokenExtensions
    {
        public static IEnumerable<int> GetOccupiedIndexes(this Token token)
        {
            var endIndex = token.StartIndex + token.Length;
            for (var i = token.StartIndex; i < endIndex; i++)
                yield return i;
        }
    }
}