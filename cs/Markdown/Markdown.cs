using Markdown.TokenConverters;
using Markdown.Tokens;

namespace Markdown
{
    public class Markdown
    {
        public string Render(string markdownText, TokenConverter converter)
        {
            var tokens = new MarkdownTokenizer().SplitTextToTokens(markdownText);
            
            var formattedText = converter.ConvertTokens(tokens);
            
            return formattedText;
        }
    }
}
