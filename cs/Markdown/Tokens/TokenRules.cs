using System.Collections.Generic;

namespace Markdown.Tokens
{
    internal class TokenRules
    {
        public static bool IsTokenTagInPosition(int i, ref string document, out List<Token> matchedTokens)
        {
            matchedTokens = new List<Token>();
            Token token;

            token = new Space();
            if (token.MatchTag(i, ref document))
                matchedTokens.Add(token);

            token = new Backslash();
            if (token.MatchTag(i, ref document))
                matchedTokens.Add(token);

            if (matchedTokens.Count > 1)
                matchedTokens.Sort((t1, t2) => t2.MarkdownTag.CompareTo(t1.MarkdownTag));

            return matchedTokens.Count > 0;
        }
    }
}
