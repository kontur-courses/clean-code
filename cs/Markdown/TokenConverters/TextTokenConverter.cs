using System.Collections.Generic;
using System.Text;

namespace Markdown.TokenConverters
{
    public class TextTokenConverter : ITokenConverter
    {
        private TokenType Type { get; }
        private string TagText { get; }

        public TextTokenConverter()
        {
            Type = TokenType.Text;
            TagText = "em";
        }

        public string ConvertTokenToString(TextToken token, IReadOnlyCollection<ITokenConverter> tokenConverters)
        {
            return token.Type != Type ? null : token.Text;
        }
    }
}