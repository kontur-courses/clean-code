using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HTMLConverter : IConverter
    {
        private readonly ITokenConverterFactory tokenConverters;

        public HTMLConverter(ITokenConverterFactory tokenConverters)
        {
            this.tokenConverters = tokenConverters;
        }

        public string ConvertTokens(IReadOnlyCollection<IToken> textTokens)
        {
            var html = new StringBuilder();
            foreach (var token in textTokens)
            {
                var tokenConverter = tokenConverters.GetTokenConverter(token.Type);
                html.Append(tokenConverter.ConvertToken(token));
            }

            return html.ToString();
        }
    }
}