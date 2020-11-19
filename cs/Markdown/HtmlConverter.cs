using System.Collections.Generic;
using System.Text;
using Markdown.Converters;
using Markdown.Tokens;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        private readonly ITokenConverterFactory converterFactory;

        public HtmlConverter(ITokenConverterFactory converterFactory)
        {
            this.converterFactory = converterFactory;
        }

        public string ConvertTokens(IEnumerable<IToken> tokens)
        {
            var html = new StringBuilder();
            foreach (var token in tokens)
            {
                var tokenConverter = converterFactory.GetTokenConverter(token.Type, this);
                html.Append(tokenConverter.ConvertToken(token));
            }

            return html.ToString();
        }
    }
}