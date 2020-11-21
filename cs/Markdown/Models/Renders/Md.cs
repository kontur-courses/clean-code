using System;
using System.Linq;
using Markdown.Models.Converters;

namespace Markdown.Models.Renders
{
    internal class Md
    {
        private readonly TokenReader reader;
        private readonly IConverter converter;

        public Md(TokenReader reader, IConverter converter)
        {
            this.reader = reader;
            this.converter = converter;
        }

        public string Render(string text)
        {
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var textTokensByParagraphs = text
                .Split(converter.NewLineRule.From.Opening)
                .Select(paragraph => reader.ReadTokens(paragraph))
                .Where(tokens => tokens.Count > 0)
                .ToList();

            return converter.Convert(textTokensByParagraphs);
        }
    }
}
