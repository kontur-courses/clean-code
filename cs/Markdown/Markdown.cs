using System.Linq;

namespace Markdown
{
    public class Markdown
    {
        private ITextParser Parser { get; }
        private IConverter Converter { get; }

        public Markdown(ITextParser parser, IConverter converter)
        {
            Parser = parser;
            Converter = converter;
        }

        public string Render(string markdown)
        {
            var tokens = Parser.GetTokens(markdown, markdown).ToList();

            return Converter.ConvertTokens(tokens);
        }
    }
}