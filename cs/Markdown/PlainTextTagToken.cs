namespace Markdown
{
    public class PlainTextTagToken : ITagToken
    {
        public string Convert(Token token)
        {
            return token.GetValueWithoutTags();
        }
    }
}