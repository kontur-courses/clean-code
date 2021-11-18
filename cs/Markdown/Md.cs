using Markdown.Converter;
using Markdown.Parser;

namespace Markdown
{
    public class Md
    {
        public string Render(string textInMarkdown)
        {
            var parser = new TextParser();
            var conveter = new TokenConverter();
            
            var tokens = parser.GetTokens(textInMarkdown);
            return conveter.ConvertTokens(tokens);
        }
    }
}