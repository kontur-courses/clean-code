using System;

namespace Markdown
{
    public class Processor
    {
        private readonly Syntax syntax;

        public Processor(Syntax syntax)
        {
            this.syntax = syntax;
        }

        public string Render(string source)
        {
            if (source == null)
                throw new ArgumentException();

            var tokens = Tokenizer.ParseText(source, syntax);
            return new HtmlConverter().ReplaceAttributesWithTags(tokens, source);
        }
    }
}