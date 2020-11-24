namespace Markdown
{
    public class HeadingTokenConverter : ITokenConverter
    {
        public string Convert(Token token, IConverter converter)
        {
            return token.ChildTokens.Count != 0
                ? $"<h1>{converter.ConvertTokens(token.ChildTokens)}</h1>"
                : $"<h1>{token.Value}</h1>";
        }
    }
}