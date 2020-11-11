using Markdown.Models.Converters;
using Markdown.Models.Syntax;


namespace Markdown.Models.Renders
{
    internal class Md
    {
        private readonly ISyntax syntax;
        private readonly IConverter converter;

        public Md(ISyntax syntax, IConverter converter)
        {
            this.syntax = syntax;
            this.converter = converter;
        }

        public string Render(string text)
        {
            var tokens = new TokenReader(syntax)
                .ReadTokens(text);

            return converter.ConvertMany(tokens);
        }
    }
}
