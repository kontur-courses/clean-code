using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Markdown.TokenConverters;

namespace Markdown
{
    public class HTMLConverter
    {
        private readonly Dictionary<TokenType, ITokenConverter> tokenConverters;

        public HTMLConverter(IReadOnlyCollection<ITokenConverter> tokenConverters)
        {
            this.tokenConverters = new Dictionary<TokenType, ITokenConverter>();
            foreach (var tokenConverter in tokenConverters)
            {
                this.tokenConverters[tokenConverter.TokenType] = tokenConverter;
            }
        }

        public HTMLConverter(Dictionary<TokenType, ITokenConverter> tokenConverters)
        {
            this.tokenConverters = tokenConverters;
        }

        public string GetHtml(IReadOnlyCollection<TextToken> textTokens)
        {
            var html = new StringBuilder();
            foreach (var token in textTokens)
            {
                var currentText = tokenConverters[token.Type].ToString(token, tokenConverters);
                if (currentText == null) continue;
                html.Append(currentText);
            }

            return html.ToString();
        }
    }
}