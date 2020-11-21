namespace Markdown
{
    public class PlainTextTagTokenConverter : ITagTokenConverter
    {
        public string Convert(Token token)
        {
            return token.Value;
        }
    }
}