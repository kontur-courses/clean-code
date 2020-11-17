using System.Collections.Generic;
using System.Linq;

namespace Markdown
{
    public static class TokenLexer
    {
        public static void FilterTokens(List<IToken> tokens)
        {
            var nonTokens = tokens.Where(token =>
                    IsDoubleUnderlineInSingle(tokens, token) ||
                    !(token is AttributeToken) && IsIntersected(tokens, token) || IsEmpty(token))
                .ToHashSet();
            tokens.RemoveAll(token => nonTokens.Contains(token));
        }

        private static bool IsDoubleUnderlineInSingle(List<IToken> tokens, IToken token)
        {
            return tokens.Any(x =>
                token != x && AreNested(token, x) && x.StartIndex < token.StartIndex && x.TagInfo.OpenTagInMd == "_" &&
                token.TagInfo.OpenTagInMd == "__");
        }

        private static bool IsIntersected(List<IToken> tokens, IToken emphasizingToken)
        {
            return tokens.Any(x =>
                emphasizingToken != x && !AreNested(emphasizingToken, x) &&
                emphasizingToken.StartIndex < x.StartIndex + x.Length &&
                x.StartIndex < emphasizingToken.StartIndex + emphasizingToken.Length);
        }

        private static bool IsEmpty(IToken emphasizingToken)
        {
            return emphasizingToken.Length == emphasizingToken.TagInfo.OpenTagInMd.Length * 2;
        }

        private static bool AreNested(IToken firstEmphasizingToken, IToken secondEmphasizingToken)
        {
            return firstEmphasizingToken.StartIndex > secondEmphasizingToken.StartIndex &&
                   firstEmphasizingToken.EndTagIndex < secondEmphasizingToken.EndTagIndex
                   || secondEmphasizingToken.StartIndex > firstEmphasizingToken.StartIndex &&
                   secondEmphasizingToken.EndTagIndex < firstEmphasizingToken.EndTagIndex;
        }
    }
}