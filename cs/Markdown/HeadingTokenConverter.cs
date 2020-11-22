namespace Markdown
{
    public class HeadingTokenConverter : ITokenConverter
    {
        public string Convert(IToken token)
        {
            return $"<h1>{token.Value}</h1>";
        }
    }
}