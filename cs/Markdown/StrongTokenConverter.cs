namespace Markdown
{
    public class StrongTokenConverter : ITokenConverter
    {
        public string Convert(Token token)
        {
            return $"<strong>{token.Value}</strong>";
        }
    }
}