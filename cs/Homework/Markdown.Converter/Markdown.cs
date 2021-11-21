using Markdown.Parser;
using Markdown.TokenFormatter;

namespace Markdown
{
    public class Markdown
    {
        private readonly IParser parser;
        private readonly ITokensFormatter tokensFormatter;

        public Markdown(IParser parser, ITokensFormatter tokensFormatter)
        {
            this.parser = parser;
            this.tokensFormatter = tokensFormatter;
        }

        public string Render(string text)
        {
            var tokens = parser.Parse(text);

            return tokensFormatter.Format(tokens);
        }
    }
}