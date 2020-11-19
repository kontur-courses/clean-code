using System.Text;
using Markdown.Tokens;

namespace Markdown.Converters
{
    public class TokenConverter : ITokenConverter
    {
        private readonly string closeTag;
        private readonly IConverter converter;
        private readonly string openTag;

        protected TokenConverter(IConverter converter, string tokenText)
        {
            this.converter = converter;
            openTag = $"<{tokenText}>";
            closeTag = $"</{tokenText}>";
        }

        public string ConvertToken(IToken token)
        {
            var htmlText = new StringBuilder();
            htmlText.Append(openTag);
            htmlText.Append(converter.ConvertTokens(token.SubTokens));
            htmlText.Append(closeTag);
            return htmlText.ToString();
        }
    }
}