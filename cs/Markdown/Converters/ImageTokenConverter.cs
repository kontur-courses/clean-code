using Markdown.Tokens;

namespace Markdown.Converters
{
    public class ImageTokenConverter : ITokenConverter
    {
        public string Convert(Token token)
        {
            var altText = token.ChildTokens[0].Value;
            var url = token.ChildTokens[1].Value;

            return $"<img src=\"{url}\" alt=\"{altText}\">";
        }
    }
}