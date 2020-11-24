namespace Markdown
{
    public class HeadingTokenConverter : ITokenConverter
    {
        public string Convert(Token token)
        {
            return $"<h1>{token.Value}</h1>";
        }
    }
}