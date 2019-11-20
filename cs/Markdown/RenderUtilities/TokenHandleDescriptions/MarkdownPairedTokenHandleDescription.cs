using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Markdown.RenderUtilities.TokenHandleDescriptions
{
    public class MarkdownPairedTokenHandleDescription : PairedTokenHandleDescription
    {
        private readonly TokenType tokenType;
        private readonly string tag;

        public override TokenType TokenType => tokenType;

        public MarkdownPairedTokenHandleDescription(TokenType tokenType, string tag)
        {
            this.tokenType = tokenType;
            this.tag = tag;
        }

        public override bool IsOpening(List<Token> tokenList, int i)
        {
            if (tokenList[i].TokenType != tokenType || i + 1 == tokenList.Count
            || tokenList[i + 1].TokenType == TokenType.WhiteSpace)
                return false;
            if (i != 0 && tokenList[i + 1].TokenType == TokenType.Digits
            && tokenList[i - 1].TokenType == TokenType.Digits)
                return false;

            return true;
        }

        public override bool IsClosing(List<Token> tokenList, int i)
        {
            if (tokenList[i].TokenType != tokenType || i == 0
            || tokenList[i - 1].TokenType == TokenType.WhiteSpace)
                return false;
            if (i + 1 != tokenList.Count && tokenList[i + 1].TokenType == TokenType.Digits
            && tokenList[i - 1].TokenType == TokenType.Digits)
                return false;

            return true;
        }

        public override string GetRenderedTokenText(Token token, bool hasPair, bool isClosed)
        {
            if (!hasPair)
                return token.Text;
            if (isClosed)
                return $"</{tag}>";
            return $"<{tag}>";
        }
    }
}
