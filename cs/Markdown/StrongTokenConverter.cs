namespace Markdown
{
    public class StrongTokenConverter : ITokenConverter
    {
        public string Convert(Token token, IConverter converter)
        {
            return token.ChildTokens.Count != 0
                ? $"<strong>{converter.ConvertTokens(token.ChildTokens)}</strong>"
                : $"<strong>{token.Value}</strong>";
        }
    }
}