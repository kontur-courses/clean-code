namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdown, ITextParser markdownParser, IConverter htmlConverter)
        {
            var tokens = markdownParser.GetTokens(markdown);

            return htmlConverter.ConvertTokens(tokens);
        }
    }
}