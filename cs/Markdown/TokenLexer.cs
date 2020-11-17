using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TokenLexer
    {
        public static void FilterTokens(List<Token> tokens)
        {
            var nonTokens = tokens.Where(token =>
                    AreDoubleUnderlineInSingle(tokens, token) || AreIntersect(tokens, token) || AreEmpty(token))
                .ToHashSet();
            tokens.RemoveAll(token => nonTokens.Contains(token));
        }

        private static bool AreDoubleUnderlineInSingle(List<Token> tokens, Token token)
        {
            return tokens.Any(x =>
                token != x && AreNested(token, x) && x.StartIndex < token.StartIndex && x.TagInfo.TagInMd == "_" &&
                token.TagInfo.TagInMd == "__");
        }

        private static bool AreIntersect(List<Token> tokens, Token token)
        {
            return tokens.Any(x =>
                token != x && !AreNested(token, x) && token.StartIndex < x.StartIndex + x.Length &&
                x.StartIndex < token.StartIndex + token.Length);
        }

        private static bool AreEmpty(Token token)
        {
            return token.Length == token.TagInfo.TagInMd.Length * 2;
        }

        private static bool AreNested(Token firstToken, Token secondToken)
        {
            return firstToken.StartIndex > secondToken.StartIndex && firstToken.EndTagIndex < secondToken.EndTagIndex
                   || secondToken.StartIndex > firstToken.StartIndex &&
                   secondToken.EndTagIndex < firstToken.EndTagIndex;
        }
    }
}