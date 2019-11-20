namespace Markdown.RenderUtilities.TokenProcessingDescriptions
{
    public class MarkdownEscapeTokenProcessingDescription : MarkdownSimpleTokenProcessingDescription
    {
        public MarkdownEscapeTokenProcessingDescription() : base(TokenType.Escape)
        { }

        public override string GetRenderedTokenText(Token token)
        {
            return token.Text.Substring(1);
        }
    }
}
