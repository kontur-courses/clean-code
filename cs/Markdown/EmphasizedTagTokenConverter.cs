namespace Markdown
{
    public class EmphasizedTagTokenConverter : ITagTokenConverter
    {
        public string Convert(Token token)
        {
            return $"<em>{token.Value}</em>";
        }
    }
}