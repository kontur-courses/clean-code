using Markdown.Converter;
using Markdown.Parser;
using Markdown.Validator;

namespace Markdown
{
    public class Md
    {
        private readonly IParser parser;
        private readonly IConverter converter;
        private readonly IValidator validator;

        public Md(IParser parser, IValidator validator, IConverter converter)
        {
            this.parser = parser;
            this.validator = validator;
            this.converter = converter;
        }
        
        public string Render(string textInMarkdown)
        {
            var tokens = parser.GetTokens(textInMarkdown);
            var validatedTokens = validator.ValidateTokens(tokens, textInMarkdown);
            return converter.ConvertTokensInText(validatedTokens, textInMarkdown);
        }
    }
}