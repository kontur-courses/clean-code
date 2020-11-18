using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HTMLConverter : IConverter
    {
        private readonly Dictionary<TokenType, ITagTokenConverter> tokenConverters;

        public HTMLConverter(Dictionary<TokenType, ITagTokenConverter> tokenConverters)
        {
            this.tokenConverters = tokenConverters;
        }

        public string ConvertTokens(IReadOnlyCollection<TextToken> textTokens)
        {
            var html = new StringBuilder();
            var converterGetter = new TokenConverterFactory();
            foreach (var token in textTokens)
            {
                var tokenConverter = converterGetter.GetTokenConverter(token.Type);
                html.Append(tokenConverter.ConvertToken(token));
            }

            return html.ToString();
        }
    }
}