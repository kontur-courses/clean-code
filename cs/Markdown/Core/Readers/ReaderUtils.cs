using System.Collections.Generic;
using System.Linq;

namespace Markdown.Core.Readers
{
    public static class ReaderUtils
    {
        public static bool IsValidPositionForOpeningTag(string word, string mdTag,
            int wordPosition, List<int> escapedPositions)
        {
            return word.StartsWith(mdTag)
                   && !Enumerable.Range(wordPosition, mdTag.Length).Any(escapedPositions.Contains);
        }

        public static bool IsValidPositionForClosingTag(string word, string mdTag,
            int closingTokenPossiblePosition, List<int> escapedPositions)
        {
            return word.EndsWith(mdTag)
                   && !Enumerable.Range(closingTokenPossiblePosition, mdTag.Length).Any(escapedPositions.Contains);
        }
    }
}