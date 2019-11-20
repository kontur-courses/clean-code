namespace Markdown.RenderUtilities.TokenProcessingDescriptions
{
    public class MarkdownSimpleTokenProcessingDescription : TokenProcessingDescription
    {
        private readonly TokenType tokenType;

        public override TokenType TokenType => tokenType;

        public MarkdownSimpleTokenProcessingDescription(TokenType tokenType)
        {
            this.tokenType = tokenType;
        }

        public virtual string GetRenderedTokenText(Token token)
        {
            return token.Text;
        }
    }
}
