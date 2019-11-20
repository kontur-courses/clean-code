using System.Collections.Generic;

namespace Markdown.RenderUtilities.TokenProcessingDescriptions
{
    public class MarkdownPairedTokenProcessingDescription : PairedTokenProcessingDescription
    {
        private readonly TokenType tokenType;
        private readonly string tag;

        public override TokenType TokenType => tokenType;

        public MarkdownPairedTokenProcessingDescription(TokenType tokenType, string tag)
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
