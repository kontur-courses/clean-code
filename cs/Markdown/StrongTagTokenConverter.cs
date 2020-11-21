namespace Markdown
{
    public class StrongTagTokenConverter : ITagTokenConverter
    {
        public string Convert(Token token)
        {
            return $"<strong>{token.Value}</strong>";
        }
    }
}