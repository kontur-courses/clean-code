using System.Collections.Generic;
using System.Text;
using Markdown.Tokens;

namespace Markdown.Converters
{
    public class HtmlConverter : IConverter
    {
        private readonly ITokenConverterFactory factory;

        public HtmlConverter(ITokenConverterFactory factory)
        {
            this.factory = factory;
        }

        public string ConvertTokens(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var converter = factory.GetTokenConverter(token.Type, this);

                result.Append(converter.Convert(token));
            }

            return result.ToString();
        }
    }
}