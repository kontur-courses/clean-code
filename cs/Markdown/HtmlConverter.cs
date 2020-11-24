using System.Collections.Generic;
using System.Text;

namespace Markdown
{
    public class HtmlConverter : IConverter
    {
        private readonly Dictionary<TokenType, ITokenConverter> mapping;

        public HtmlConverter(Dictionary<TokenType, ITokenConverter> mapping)
        {
            this.mapping = mapping;
        }

        public string ConvertTokens(IEnumerable<Token> tokens)
        {
            var result = new StringBuilder();

            foreach (var token in tokens)
            {
                var converter = mapping[token.Type];
                var newConverter = new HtmlConverter(mapping);

                result.Append(converter.Convert(token, newConverter));
            }

            return result.ToString();
        }
    }
}