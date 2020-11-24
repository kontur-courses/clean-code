namespace Markdown
{
    public class PlainTextTokenConverter : ITokenConverter
    {
        public string Convert(Token token, IConverter converter)
        {
            return token.Value;
        }
    }
}