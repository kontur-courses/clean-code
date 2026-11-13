using System.Collections.Generic;
using System.Linq;
using Markdown.Entity;
using Markdown.Parser.Inline.Tokens;

namespace Markdown.Parser.Inline
{
    public static class PairRules
    {
        public static bool CanOpen(PairToken token, IEnumerable<PairToken> openTokens)
        {
            if (!token.CanOpen) return false;

            if (token is StrongToken && openTokens.Any(t => t is EmToken))
                return false;
            return true;
        }

        public static bool CanClose(PairToken openToken, IReadOnlyList<INode> children, PairToken closingToken)
        {
            if (!closingToken.CanClose) return false;

            if (children.Count == 0)
                return false;

            if (openToken.IsInsideWord && closingToken.IsInsideWord)
            {
                if (HasWhitespaceInside(children))
                    return false;
            }

            return true;
        }

        public static bool IsIntersection(PairToken closingToken, IEnumerable<PairToken> openTokens) => 
            openTokens.Any(t => t.GetType() == closingToken.GetType());

        private static bool HasWhitespaceInside(IReadOnlyList<INode> children) => 
            children.Any(c => c is TextNode t && t.Text.Any(char.IsWhiteSpace));
    }
}