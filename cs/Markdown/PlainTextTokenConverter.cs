namespace Markdown
{
    public class PlainTextTokenConverter : ITokenConverter
    {
        public string Convert(Token token)
        {
            return token.Value;
        }
    }
}