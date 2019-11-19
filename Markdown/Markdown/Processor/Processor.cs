using System;

namespace Markdown
{
    public class Processor
    {
        private readonly ISyntax syntax;
        private readonly IConverter converter;

        public Processor(ISyntax syntax, IConverter converter)
        {
            this.syntax = syntax;
            this.converter = converter;
        }

        public string Render(string source)
        {
            if (source == null)
                throw new ArgumentException();

            var tokenizer = new Tokenizer(syntax);
            var tokens = tokenizer.ParseText(source);

            return converter.ReplaceAttributesWithTags(tokens, source);
        }
    }
}