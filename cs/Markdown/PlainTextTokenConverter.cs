namespace Markdown
{
    public class PlainTextTokenConverter : ITokenConverter
    {
        public string Convert(IToken token)
        {
            return token.Value;
        }
    }
}