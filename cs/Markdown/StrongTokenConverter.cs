namespace Markdown
{
    public class StrongTokenConverter : ITokenConverter
    {
        public string Convert(IToken token)
        {
            return $"<strong>{token.Value}</strong>";
        }
    }
}