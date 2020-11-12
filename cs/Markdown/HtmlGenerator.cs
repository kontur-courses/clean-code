using System.Collections.Generic;

namespace Markdown
{
    public class HtmlGenerator
    {
        private IConverter Converter { get; }

        public HtmlGenerator(IConverter converter)
        {
            Converter = converter;
        }

        public string CovertTokensToHtml(List<Token> tokens)
        {
            return Converter.CovertTokensToHtml(tokens);
        }
    }
}