using System;
using System.Text;
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
            if (text == null)
                throw new ArgumentNullException(nameof(text));

            var convertedText = new StringBuilder();
            var reader = new TokenReader(syntax);

            var isFirst = true;
            foreach (var paragraph in text.Split("\n"))
            {
                var paragraphTokens = reader.ReadTokens(paragraph);
                var convertedParagraph = converter.Convert(paragraphTokens, !isFirst);
                convertedText.Append(convertedParagraph);
                isFirst = false;
            }

            return convertedText.ToString();
        }
    }
}
