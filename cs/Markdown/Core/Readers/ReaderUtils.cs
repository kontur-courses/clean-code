using System.Collections.Generic;
using System.Linq;

namespace Markdown.Core.Readers
{
    public static class ReaderUtils
    {
        public static bool IsValidPositionForOpeningTag(string word, string tag,
            int wordPosition, List<int> escapedPositions)
        {
            return word.StartsWith(tag)
                   && !Enumerable.Range(wordPosition, tag.Length).Any(escapedPositions.Contains);
        }

        public static bool IsValidPositionForClosingTag(string word, string inlineTag,
            int closingTokenPossiblePosition, List<int> escapedPositions)
        {
            return word.EndsWith(inlineTag)
                   && !Enumerable.Range(closingTokenPossiblePosition, inlineTag.Length).Any(escapedPositions.Contains);
        }
    }
}