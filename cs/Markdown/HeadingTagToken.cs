namespace Markdown
{
    public class HeadingTagToken : ITagToken
    {
        public string Convert(Token token)
        {
            return $"<h1>{token.GetValueWithoutTags()}</h1>";
        }
    }
}