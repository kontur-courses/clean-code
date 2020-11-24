namespace Markdown
{
    public class EmphasizedTokenConverter : ITokenConverter
    {
        public string Convert(Token token, IConverter converter)
        {
            return token.ChildTokens.Count != 0
                ? $"<em>{converter.ConvertTokens(token.ChildTokens)}</em>"
                : $"<em>{token.Value}</em>";
        }
    }
}