using Markdown.Parser;
using Markdown.TokenFormatter;

namespace Markdown
{
    public class Md
    {
        private readonly IParser parser;
        private readonly ITokenFormatter tokenFormatter;

        public Md(IParser parser, ITokenFormatter tokenFormatter)
        {
            this.parser = parser;
            this.tokenFormatter = tokenFormatter;
        }
        
        public string Render(string text)
        {
            var tokens = parser.Parse(text);

            return tokenFormatter.Format(tokens);
        }
    }
}