namespace Markdown
{
    public class ImageTagTokenConverter : ITagTokenConverter
    {
        public string Convert(Token token)
        {
            var altText = token.ChildTokens[0].Value;
            var url = token.ChildTokens[1].Value;

            return $"<img src=\"{url}\" alt=\"{altText}\">";
        }
    }
}