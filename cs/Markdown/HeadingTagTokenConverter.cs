namespace Markdown
{
    public class HeadingTagTokenConverter : ITagTokenConverter
    {
        public string Convert(Token token)
        {
            return $"<h1>{token.Value}</h1>";
        }
    }
}