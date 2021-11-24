using Markdown.Converter;
using Markdown.Parser;
using Markdown.Validator;

namespace Markdown
{
    public class Md
    {
        private readonly IParser parser;
        private readonly IConverter converter;

        public Md(IParser parser, IConverter converter)
        {
            this.parser = parser;
            this.converter = converter;
        }
        
        public string Render(string textInMarkdown)
        {
            var tokens = parser.GetTokens(textInMarkdown);
            var validatedTokens = TokenValidator.ValidateTokens(tokens, textInMarkdown);
            return converter.ConvertTokensInText(validatedTokens, textInMarkdown);
        }
    }
}