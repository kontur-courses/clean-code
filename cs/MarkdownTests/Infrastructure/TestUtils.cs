using System.Collections.Generic;
using Markdown.Core.Tokens;

namespace MarkdownTests.Infrastructure
{
    public static class TestUtils
    {
        public static bool IsSameTokenLists(List<Token> firstTokens, List<Token> secondTokens)
        {
            if (firstTokens.Count != secondTokens.Count)
                return false;

            for (var i = 0; i < firstTokens.Count; i++)
            {
                var first = firstTokens[i];
                var second = secondTokens[i];
                if (!first.Equals(second))
                    return false;
            }

            return true;
        }
    }
}