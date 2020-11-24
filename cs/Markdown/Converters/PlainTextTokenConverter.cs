using Markdown.Tokens;

namespace Markdown.Converters
{
    public class PlainTextTokenConverter : ITokenConverter
    {
        public string Convert(Token token)
        {
            return token.Value;
        }
    }
}