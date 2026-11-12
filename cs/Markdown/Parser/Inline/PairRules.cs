using System.Collections.Generic;
using System.Linq;
using Markdown.Entity;
using Markdown.Parser.Inline.Tokens;

namespace Markdown.Parser.Inline
{
    public static class PairRules
    {
        public static bool CanOpen(Token token, IEnumerable<Token> openTokens)
        {
            if (!token.CanOpen) return false;

            if (token.Type == TokenType.Strong && openTokens.Any(t => t.Type == TokenType.Em))
                return false;

            return true;
        }

        public static bool CanClose(Token openToken, IReadOnlyList<INode> children, Token closingToken)
        {
            if (!closingToken.CanClose) return false;

            if (children.Count == 0) return false;

            if (openToken.IsInsideWord && closingToken.IsInsideWord)
            {
                if (HasWhitespaceInside(children)) return false;
            }

            return true;
        }

        public static bool IsIntersection(Token closingToken, IEnumerable<Token> openTokens) => 
            openTokens.Any(t => t.Type == closingToken.Type);

        private static bool HasWhitespaceInside(IReadOnlyList<INode> children) => 
            children.Any(c => c is TextNode t && t.Text.Any(char.IsWhiteSpace));
    }
}