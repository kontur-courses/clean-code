using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown
{
    static class RuleForTagsAdding
    {
        public static bool IsNeedToAddTags(SyntaxTree tree, int tokenIndex)
        {
            if (!IsCorrectTagStart(tree.Parent.Tokens, tokenIndex - 1) ||
                !IsCorrectTagEnd(tree.Parent.Tokens, tokenIndex + tree.Tokens.Count) ||
                tree.Tokens.Count <= 0)
                return false;
            return true;
        }

        private static bool IsCorrectTagStart(List<Token> tokens, int tokenIndex)
        {
            if (tokenIndex >= tokens.Count - 1)
                return false;
            if (tokenIndex > 0 && tokens[tokenIndex - 1].TokenType == TokenType.Number && tokens[tokenIndex + 1].TokenType == TokenType.Number)
                return false;
            if (tokens[tokenIndex + 1].TokenType == TokenType.Whitespaces)
                return false;
            return true;
        }

        private static bool IsCorrectTagEnd(List<Token> tokens, int tokenIndex)
        {
            if (tokenIndex <= 0)
                return false;
            if (tokens[tokenIndex - 1].TokenType == TokenType.Whitespaces)
                return false;
            if (tokenIndex < tokens.Count - 1 && tokens[tokenIndex - 1].TokenType == TokenType.Number && tokens[tokenIndex + 1].TokenType == TokenType.Number)
                return false;
            return true;
        }
    }
}
