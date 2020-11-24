namespace Markdown
{
    public class EmphasizedTokenConverter : ITokenConverter
    {
        public string Convert(Token token)
        {
            return $"<em>{token.Value}</em>";
        }
    }
}