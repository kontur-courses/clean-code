namespace Markdown
{
    public class EmphasizedTokenConverter : ITokenConverter
    {
        public string Convert(IToken token)
        {
            return $"<em>{token.Value}</em>";
        }
    }
}