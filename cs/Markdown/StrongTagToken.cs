namespace Markdown
{
    public class StrongTagToken : ITagToken
    {
        public string Convert(Token token)
        {
            return $"<strong>{token.GetValueWithoutTags()}</strong>";
        }
    }
}