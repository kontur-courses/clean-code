using Markdown.Tokens;

namespace Markdown.Converters
{
    public class PlainTextTokenConverter : ITokenConverter
    {
        public string ConvertToken(IToken token)
        {
            return token.Text;
        }
    }
}